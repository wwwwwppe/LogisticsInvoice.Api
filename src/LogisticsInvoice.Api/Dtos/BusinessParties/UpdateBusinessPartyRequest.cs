using System.ComponentModel.DataAnnotations;
using LogisticsInvoice.Api.Entities;

namespace LogisticsInvoice.Api.Dtos.BusinessParties;

public class UpdateBusinessPartyRequest
{
    [Required(ErrorMessage = "客户/供应商名称不能为空")]
    [StringLength(100, ErrorMessage = "名称长度不能超过 100 个字符")]
    public string Name { get; set; } = string.Empty;

    [EnumDataType(typeof(BusinessPartyType), ErrorMessage = "类型只能是 Customer、Supplier 或 Both")]
    public BusinessPartyType Type { get; set; }

    [StringLength(50, ErrorMessage = "联系人长度不能超过 50 个字符")]
    public string? ContactName { get; set; }

    [StringLength(30, ErrorMessage = "联系电话长度不能超过 30 个字符")]
    public string? Phone { get; set; }

    [StringLength(200, ErrorMessage = "地址长度不能超过 200 个字符")]
    public string? Address { get; set; }
}
