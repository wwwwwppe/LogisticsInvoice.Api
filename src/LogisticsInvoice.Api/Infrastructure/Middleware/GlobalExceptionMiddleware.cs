using System.Net;
using LogisticsInvoice.Api.Common;

namespace LogisticsInvoice.Api.Infrastructure.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BusinessException exception)
        {
            _logger.LogWarning(
                exception,
                "业务请求失败。TraceId: {TraceId}",
                context.TraceIdentifier);

            await WriteResponseAsync(context, exception.StatusCode, exception.Message);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "未处理异常。TraceId: {TraceId}",
                context.TraceIdentifier);

            await WriteResponseAsync(
                context,
                (int)HttpStatusCode.InternalServerError,
                "服务器内部错误，请稍后重试");
        }
    }

    private static async Task WriteResponseAsync(
        HttpContext context,
        int statusCode,
        string message)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json; charset=utf-8";

        var response = ApiResponse<object>.Fail(
            message,
            traceId: context.TraceIdentifier);

        await context.Response.WriteAsJsonAsync(response);
    }
}
