using LogisticsInvoice.Api.Common;
using LogisticsInvoice.Api.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LogisticsInvoice.Api.Controllers;

[ApiController]
[Route("api/health")]
public class HealthController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public HealthController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<object>>> Get()
    {
        var databaseAvailable = await _dbContext.Database.CanConnectAsync();
        var data = new
        {
            Status = databaseAvailable ? "Healthy" : "Degraded",
            Database = databaseAvailable ? "Connected" : "Unavailable",
            Timestamp = DateTime.UtcNow
        };

        return Ok(ApiResponse<object>.Ok(data, "服务运行正常"));
    }
}
