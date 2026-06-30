using LogisticsInvoice.Api.Common;
using LogisticsInvoice.Api.Dtos.BusinessParties;
using LogisticsInvoice.Api.Entities;
using LogisticsInvoice.Api.Repositories;

namespace LogisticsInvoice.Api.Services;

public class BusinessPartyService : IBusinessPartyService
{
    private const int MaxPageSize = 100;
    private readonly IBusinessPartyRepository _repository;

    public BusinessPartyService(IBusinessPartyRepository repository)
    {
        _repository = repository;
    }

    public async Task<BusinessPartyDto> CreateAsync(CreateBusinessPartyRequest request)
    {
        var code = request.Code.Trim().ToUpperInvariant();
        if (await _repository.CodeExistsAsync(code))
        {
            throw new BusinessException($"编码 {code} 已存在");
        }

        var businessParty = new BusinessParty
        {
            Code = code,
            Name = request.Name.Trim(),
            Type = request.Type,
            ContactName = NormalizeOptional(request.ContactName),
            Phone = NormalizeOptional(request.Phone),
            Address = NormalizeOptional(request.Address),
            IsEnabled = true,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(businessParty);
        await _repository.SaveChangesAsync();

        return ToDto(businessParty);
    }

    public Task<BusinessPartyDto> GetByIdAsync(int id)
    {
        // TODO(学习任务 1):
        // 1. 根据 id 查询客户/供应商。
        // 2. 数据不存在时返回符合现有规范的 404 业务异常。
        // 3. 将 Entity 转换为 BusinessPartyDto 后返回。
        // 提示：优先观察本类已有的私有方法，避免复制同一段逻辑。
        throw new NotImplementedException("请完成学习任务 1：按 ID 查询客户/供应商");
    }

    public async Task<BusinessPartyDto> UpdateAsync(
        int id,
        UpdateBusinessPartyRequest request)
    {
        var businessParty = await GetRequiredAsync(id);

        businessParty.Name = request.Name.Trim();
        businessParty.Type = request.Type;
        businessParty.ContactName = NormalizeOptional(request.ContactName);
        businessParty.Phone = NormalizeOptional(request.Phone);
        businessParty.Address = NormalizeOptional(request.Address);
        businessParty.UpdatedAt = DateTime.UtcNow;

        await _repository.SaveChangesAsync();
        return ToDto(businessParty);
    }

    public async Task<PagedResult<BusinessPartyDto>> GetPagedAsync(BusinessPartyQuery query)
    {
        if (query.PageNumber < 1)
        {
            throw new BusinessException("页码必须大于或等于 1");
        }

        if (query.PageSize < 1 || query.PageSize > MaxPageSize)
        {
            throw new BusinessException($"每页条数必须在 1 到 {MaxPageSize} 之间");
        }

        var (items, totalCount) = await _repository.GetPagedAsync(
            query.Keyword,
            query.Type,
            query.IsEnabled,
            query.PageNumber,
            query.PageSize);

        return new PagedResult<BusinessPartyDto>
        {
            Items = items.Select(ToDto).ToList(),
            TotalCount = totalCount,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize
        };
    }

    public async Task SetStatusAsync(int id, bool isEnabled)
    {
        var businessParty = await GetRequiredAsync(id);

        businessParty.IsEnabled = isEnabled;
        businessParty.UpdatedAt = DateTime.UtcNow;

        await _repository.SaveChangesAsync();
    }

    private async Task<BusinessParty> GetRequiredAsync(int id)
    {
        var businessParty = await _repository.GetByIdAsync(id);
        if (businessParty is null)
        {
            throw new BusinessException(
                $"未找到 ID 为 {id} 的客户/供应商",
                StatusCodes.Status404NotFound);
        }

        return businessParty;
    }

    private static string? NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    private static BusinessPartyDto ToDto(BusinessParty entity)
    {
        return new BusinessPartyDto
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name,
            Type = entity.Type,
            ContactName = entity.ContactName,
            Phone = entity.Phone,
            Address = entity.Address,
            IsEnabled = entity.IsEnabled,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}
