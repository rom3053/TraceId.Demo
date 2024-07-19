using TraceId.Demo.MassTransit.Services;
using TraceId.Demo.TraceIdHttpClient;

namespace TraceId.Demo.Services;

public class DemoHttpClient
{
    protected readonly HttpClient _httpClientWithHandler;
    protected readonly HttpClient _httpClient;
    private readonly TraceIdHttpClientHandler _traceIdHttpClientHandler;
    private readonly TraceIdService _traceIdService;

    public DemoHttpClient(TraceIdHttpClientHandler traceIdHttpClientHandler,
        TraceIdService traceIdService)
    {
        _httpClientWithHandler = new HttpClient(traceIdHttpClientHandler);
        _traceIdHttpClientHandler = traceIdHttpClientHandler;

        _traceIdService = traceIdService;
        _httpClient = new HttpClient().AddTraceIdHeader(_traceIdService);
    }

    public async Task SendViaHandlerAsync()
    {
        await _httpClientWithHandler.GetAsync("https://localhost:7144/api/HttpClientTraceIdDemo/TestTraceIdHttpClient");
    }

    public async Task SendViaExtensionAsync()
    {
        await _httpClient.GetAsync("https://localhost:7144/api/HttpClientTraceIdDemo/TestTraceIdHttpClient");
    }
}
