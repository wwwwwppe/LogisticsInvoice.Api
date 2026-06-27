using LogisticsInvoice.Api.Entities;

namespace LogisticsInvoice.Api.Dtos.BusinessParties;

public class BusinessPartyQuery
{
    public string? Keyword { get; set; }

    public BusinessPartyType? Type { get; set; }

    public bool? IsEnabled { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}
