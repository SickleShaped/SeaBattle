using System;
using System.Net;

namespace SeaBattle.Extensions;

public class MiddlewareBuilderService
{
    private readonly RequestDelegate _next;
    private readonly ILogger<MiddlewareBuilderService> _logger;

    public MiddlewareBuilderService(RequestDelegate next, ILogger<MiddlewareBuilderService> logger)
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
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }


    private async Task HandleException(HttpContext context, Exception ex)
    {
        _logger.LogError(ex, "An unexpected error occurred.");
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    }
}