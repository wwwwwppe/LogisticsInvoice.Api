using LogisticsInvoice.Api.Common;
using LogisticsInvoice.Api.Dtos.BusinessParties;

namespace LogisticsInvoice.Api.Services;

public interface IBusinessPartyService
{
    Task<BusinessPartyDto> CreateAsync(CreateBusinessPartyRequest request);

    Task<BusinessPartyDto> GetByIdAsync(int id);

    Task<BusinessPartyDto> UpdateAsync(int id, UpdateBusinessPartyRequest request);

    Task<PagedResult<BusinessPartyDto>> GetPagedAsync(BusinessPartyQuery query);

    Task SetStatusAsync(int id, bool isEnabled);
}
