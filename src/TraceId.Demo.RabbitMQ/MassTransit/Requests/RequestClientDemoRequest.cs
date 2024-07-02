using MassTransit;

namespace TraceId.Demo.RabbitMQ.MassTransit.Requests;

[MessageUrn(nameof(RequestClientDemoRequest))]
public class RequestClientDemoRequest
{
    public string? Message { get; set; }
}
