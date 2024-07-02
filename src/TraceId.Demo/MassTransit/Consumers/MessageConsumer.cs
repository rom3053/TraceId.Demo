using MassTransit;
using TraceId.Demo.Dtos.MassTransit;

namespace TraceId.Demo.MassTransit.Consumers;

public class MessageConsumer : IConsumer<ConsumerMessageRequest>
{
    private readonly ILogger<MessageConsumer> _logger;

    public MessageConsumer(ILogger<MessageConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ConsumerMessageRequest> context)
    {
        try
        {
            _logger.LogInformation("We get the next data:" + context.Message.Data);

            return;
        }
        catch (Exception)
        {

            throw;
        }
    }
}
