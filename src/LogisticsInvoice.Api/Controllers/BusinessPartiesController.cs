using LogisticsInvoice.Api.Common;
using LogisticsInvoice.Api.Dtos.BusinessParties;
using LogisticsInvoice.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsInvoice.Api.Controllers;

[ApiController]
[Route("api/business-parties")]
public class BusinessPartiesController : ControllerBase
{
    private readonly IBusinessPartyService _service;

    public BusinessPartiesController(IBusinessPartyService service)
    {
        _service = service;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<BusinessPartyDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<BusinessPartyDto>>> Create(
        CreateBusinessPartyRequest request)
    {
        var result = await _service.CreateAsync(request);
        return StatusCode(
            StatusCodes.Status201Created,
            ApiResponse<BusinessPartyDto>.Ok(result, "新增成功"));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<BusinessPartyDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<BusinessPartyDto>>> GetById(int id)
    {
        // TODO(学习任务 1):
        // 调用 Service，使用 ApiResponse<T> 包装结果，并返回 HTTP 200。
        // 不要在 Controller 中直接访问 Repository 或 DbContext。
        var result =await _service.GetByIdAsync(id);
        return Ok(ApiResponse<BusinessPartyDto>.Ok(result));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<BusinessPartyDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<BusinessPartyDto>>> Update(
        int id,
        UpdateBusinessPartyRequest request)
    {
        var result = await _service.UpdateAsync(id, request);
        return Ok(ApiResponse<BusinessPartyDto>.Ok(result, "修改成功"));
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(ApiResponse<PagedResult<BusinessPartyDto>>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<BusinessPartyDto>>>> GetPaged(
        [FromQuery] BusinessPartyQuery query)
    {
        var result = await _service.GetPagedAsync(query);
        return Ok(ApiResponse<PagedResult<BusinessPartyDto>>.Ok(result));
    }

    [HttpPatch("{id:int}/status")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object>>> SetStatus(
        int id,
        SetBusinessPartyStatusRequest request)
    {
        await _service.SetStatusAsync(id, request.IsEnabled);
        var message = request.IsEnabled ? "启用成功" : "停用成功";
        return Ok(ApiResponse<object>.Ok(new { Id = id, request.IsEnabled }, message));
    }
}
