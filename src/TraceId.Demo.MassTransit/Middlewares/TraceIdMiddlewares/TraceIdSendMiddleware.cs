using MassTransit;
using TraceId.Demo.Constants;
using TraceId.Demo.MassTransit.Services;

namespace TraceId.Demo.MassTransit.Middlewares.TraceIdMiddlewares;

public class TraceIdSendMiddleware<T> : IFilter<SendContext<T>> where T : class
{
    private readonly TraceIdService _traceIdService;

    public TraceIdSendMiddleware(TraceIdService traceIdService)
    {
        _traceIdService = traceIdService;
    }

    public async Task Send(SendContext<T> context, IPipe<SendContext<T>> next)
    {
        var traceId = _traceIdService.TraceId;

        if (_traceIdService.TraceIdIsNotNullOrEmpty())
        {
            Serilog.Context.LogContext.PushProperty(LoggerConstants.LoggerHeaders.TracerIdHeader, traceId);
            context.Headers.Append(new HeaderValue(LoggerConstants.LoggerHeaders.TracerIdHeader, traceId));
        }
        else if (!context.Headers.TryGetHeader(LoggerConstants.LoggerHeaders.TracerIdHeader, out var massTraceId))
        {
            traceId = Guid.NewGuid().ToString();
            context.Headers.Set(LoggerConstants.LoggerHeaders.TracerIdHeader, traceId);
        }

        await next.Send(context);
    }

    public void Probe(ProbeContext context)
    {
    }
}