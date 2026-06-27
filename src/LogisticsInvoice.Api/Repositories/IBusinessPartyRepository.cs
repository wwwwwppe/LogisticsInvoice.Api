using LogisticsInvoice.Api.Entities;

namespace LogisticsInvoice.Api.Repositories;

public interface IBusinessPartyRepository
{
    Task AddAsync(BusinessParty businessParty);

    Task<BusinessParty?> GetByIdAsync(int id);

    Task<bool> CodeExistsAsync(string code);

    Task<(IReadOnlyList<BusinessParty> Items, int TotalCount)> GetPagedAsync(
        string? keyword,
        BusinessPartyType? type,
        bool? isEnabled,
        int pageNumber,
        int pageSize);

    Task SaveChangesAsync();
}
