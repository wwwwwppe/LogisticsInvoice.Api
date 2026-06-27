using LogisticsInvoice.Api.Entities;
using LogisticsInvoice.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LogisticsInvoice.Api.Repositories;

public class BusinessPartyRepository : IBusinessPartyRepository
{
    private readonly AppDbContext _dbContext;

    public BusinessPartyRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(BusinessParty businessParty)
    {
        await _dbContext.BusinessParties.AddAsync(businessParty);
    }

    public Task<BusinessParty?> GetByIdAsync(int id)
    {
        return _dbContext.BusinessParties.FirstOrDefaultAsync(item => item.Id == id);
    }

    public Task<bool> CodeExistsAsync(string code)
    {
        return _dbContext.BusinessParties.AnyAsync(item => item.Code == code);
    }

    public async Task<(IReadOnlyList<BusinessParty> Items, int TotalCount)> GetPagedAsync(
        string? keyword,
        BusinessPartyType? type,
        bool? isEnabled,
        int pageNumber,
        int pageSize)
    {
        var query = _dbContext.BusinessParties
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var value = keyword.Trim();
            query = query.Where(item =>
                item.Code.Contains(value) || item.Name.Contains(value));
        }

        if (type.HasValue)
        {
            query = query.Where(item => item.Type == type.Value);
        }

        if (isEnabled.HasValue)
        {
            query = query.Where(item => item.IsEnabled == isEnabled.Value);
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderByDescending(item => item.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
