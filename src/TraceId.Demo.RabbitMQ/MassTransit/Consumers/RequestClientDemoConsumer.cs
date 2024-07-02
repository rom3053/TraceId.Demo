using MassTransit;
using TraceId.Demo.MassTransit.Services;
using TraceId.Demo.RabbitMQ.MassTransit.Requests;
using TraceId.Demo.RabbitMQ.MassTransit.Responses;

namespace TraceId.Demo.RabbitMQ.MassTransit.Consumers;

public sealed class RequestClientDemoConsumer : IConsumer<RequestClientDemoRequest>
{
    private readonly ILogger<RequestClientDemoConsumer> _logger;
    private readonly TraceIdService _traceIdService;

    public RequestClientDemoConsumer(ILogger<RequestClientDemoConsumer> logger,
        TraceIdService traceIdService)
    {
        _logger = logger;
        _traceIdService = traceIdService;
    }

    public async Task Consume(ConsumeContext<RequestClientDemoRequest> context)
    {
        var msg = context.Message;

        try
        {
            await context.RespondAsync(new RequestClientDemoResponse
            {
                Message = _traceIdService.TraceId,
            });
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error processing request {Message}; Stack trace: {StackTrace}");
        }
    }
}