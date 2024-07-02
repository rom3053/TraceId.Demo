using Serilog.Context;
using TraceId.Demo.Constants;
using TraceId.Demo.MassTransit.Services;

namespace WebAPI.Middlewares;

public class LogTraceIdMiddleware
{
    //X-Tracer-ID
    private const string TraceIdHeader = LoggerConstants.LoggerHeaders.TracerIdHeader;

    private readonly RequestDelegate _next;

    public LogTraceIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, TraceIdService _traceIdService)
    {
        if (!context.Request.Headers.TryGetValue(TraceIdHeader, out var traceId))
        {
            traceId = Guid.NewGuid().ToString();
            context.Request.Headers.Add(TraceIdHeader, traceId);
            _traceIdService.TraceId = traceId;
        }

        context.Response.OnStarting(() =>
        {
            context.Response.Headers.Add(TraceIdHeader, _traceIdService.TraceId);
            return Task.CompletedTask;
        });

        using (LogContext.PushProperty(LoggerConstants.LoggerProperties.TraceId, traceId.ToString()))
        {
            await _next(context);
        }
    }
}

public static class LogTraceIdMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestLogTraceId(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LogTraceIdMiddleware>();
    }
}
