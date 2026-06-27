using LogisticsInvoice.Api.Entities;

namespace LogisticsInvoice.Api.Dtos.BusinessParties;

public class BusinessPartyDto
{
    public int Id { get; init; }

    public string Code { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public BusinessPartyType Type { get; init; }

    public string? ContactName { get; init; }

    public string? Phone { get; init; }

    public string? Address { get; init; }

    public bool IsEnabled { get; init; }

    public DateTime CreatedAt { get; init; }

    public DateTime? UpdatedAt { get; init; }
}
