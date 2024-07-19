using TraceId.Demo.Constants;
using TraceId.Demo.MassTransit.Services;

namespace TraceId.Demo.TraceIdHttpClient;

public class TraceIdHttpClientHandler : HttpClientHandler
{   
    //X-Tracer-ID
    private const string TraceIdHeader = LoggerConstants.LoggerHeaders.TracerIdHeader;

    private readonly TraceIdService _traceIdService;

    public TraceIdHttpClientHandler(TraceIdService traceIdService)
    {
        _traceIdService = traceIdService;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (_traceIdService.TraceIdIsNotNullOrEmpty())
        {
            request.Headers.Add(TraceIdHeader, _traceIdService.TraceId);
        }
        else
        {
            var traceId = Guid.NewGuid().ToString();
            request.Headers.Add(TraceIdHeader, traceId);
            _traceIdService.TraceId = traceId;
        }

        return base.SendAsync(request, cancellationToken);
    }
}
