using TraceId.Demo.Constants;
using TraceId.Demo.MassTransit.Services;

namespace TraceId.Demo.TraceIdHttpClient;

public static class HttpClientExtensions
{
    public static HttpClient AddTraceIdHeader(this HttpClient client, TraceIdService traceIdService)
    {
        if (traceIdService.TraceIdIsNotNullOrEmpty())
        {
            client.DefaultRequestHeaders.Add(LoggerConstants.LoggerHeaders.TracerIdHeader, traceIdService.TraceId);
        }
        else
        {
            traceIdService.TraceId = TraceIdService.GeneratedTraceId();
            client.DefaultRequestHeaders.Add(LoggerConstants.LoggerHeaders.TracerIdHeader, traceIdService.TraceId);
        }

        return client;
    }
}
