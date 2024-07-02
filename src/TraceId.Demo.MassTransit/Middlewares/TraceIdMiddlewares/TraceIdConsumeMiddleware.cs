using MassTransit;
using TraceId.Demo.Constants;
using TraceId.Demo.MassTransit.Services;

namespace TraceId.Demo.MassTransit.Middlewares.TraceIdMiddlewares;

public class TraceIdConsumeMiddleware<T> : IFilter<ConsumeContext<T>> where T : class
{
    private readonly TraceIdService _traceIdService;

    public TraceIdConsumeMiddleware(TraceIdService traceIdService)
    {
        _traceIdService = traceIdService;
    }

    public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        if (context.Headers.TryGetHeader(LoggerConstants.LoggerHeaders.TracerIdHeader, out var massTraceId))
        {
            _traceIdService.TraceId = (string)massTraceId;
            Serilog.Context.LogContext.PushProperty(LoggerConstants.LoggerProperties.TraceId, massTraceId);
        }
        else if (_traceIdService.TraceIdIsNotNullOrEmpty())
        {
            var traceId = _traceIdService.TraceId;
            Serilog.Context.LogContext.PushProperty(LoggerConstants.LoggerProperties.TraceId, traceId);
            context.Headers.Append(new HeaderValue(LoggerConstants.LoggerHeaders.TracerIdHeader, traceId));
        }
        else
        {
            var generatedTraceId = Guid.NewGuid().ToString();
            Serilog.Context.LogContext.PushProperty(LoggerConstants.LoggerProperties.TraceId, generatedTraceId);
            context.Headers.Append(new HeaderValue(LoggerConstants.LoggerHeaders.TracerIdHeader, generatedTraceId));
            _traceIdService.TraceId = generatedTraceId;
        }

        await next.Send(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateFilterScope("traceId-consume-middleware");
    }
}
