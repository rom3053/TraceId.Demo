using MassTransit;
using TraceId.Demo.Constants;
using TraceId.Demo.MassTransit.Services;

namespace TraceId.Demo.MassTransit.Middlewares.TraceIdMiddlewares;

public class TraceIdPublishMiddleware<T> : IFilter<PublishContext<T>> where T : class
{
    private readonly TraceIdService _traceIdService;

    public TraceIdPublishMiddleware(TraceIdService traceIdService)
    {
        _traceIdService = traceIdService;
    }

    public async Task Send(PublishContext<T> context, IPipe<PublishContext<T>> next)
    {
        var traceId = _traceIdService.TraceId;

        if (_traceIdService.TraceIdIsNotNullOrEmpty())
        {
            Serilog.Context.LogContext.PushProperty(LoggerConstants.LoggerHeaders.TracerIdHeader, traceId);
            context.Headers.Set(LoggerConstants.LoggerHeaders.TracerIdHeader, traceId);
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