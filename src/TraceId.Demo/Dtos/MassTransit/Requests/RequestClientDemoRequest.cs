using MassTransit;

namespace TraceId.Demo.Dtos.MassTransit.Requests;

[MessageUrn(nameof(RequestClientDemoRequest))]
public class RequestClientDemoRequest
{
    public string? Message { get; set; }
}
