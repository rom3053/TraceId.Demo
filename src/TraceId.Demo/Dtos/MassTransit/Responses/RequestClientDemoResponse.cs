using MassTransit;

namespace TraceId.Demo.Dtos.MassTransit.Responses;

[MessageUrn(nameof(RequestClientDemoResponse))]
public class RequestClientDemoResponse
{
    public string? Message { get; set; }
}
