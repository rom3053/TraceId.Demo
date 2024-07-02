namespace TraceId.Demo.MassTransit.Services;

public class TraceIdService
{
    public string? TraceId { get; set; }

    public bool TraceIdIsNotNullOrEmpty() => !string.IsNullOrEmpty(TraceId);
}