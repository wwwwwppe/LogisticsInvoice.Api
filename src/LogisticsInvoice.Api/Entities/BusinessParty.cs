namespace LogisticsInvoice.Api.Entities;

public class BusinessParty
{
    public int Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public BusinessPartyType Type { get; set; }

    public string? ContactName { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public bool IsEnabled { get; set; } = true;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
