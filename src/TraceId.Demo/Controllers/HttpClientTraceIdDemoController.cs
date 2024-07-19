using Microsoft.AspNetCore.Mvc;
using TraceId.Demo.Constants;
using TraceId.Demo.Services;

namespace TraceId.Demo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HttpClientTraceIdDemoController : ControllerBase
{
    private readonly DemoHttpClient _demoHttpClient;
    private readonly ILogger<HttpClientTraceIdDemoController> _logger;

    public HttpClientTraceIdDemoController(DemoHttpClient demoHttpClient, 
        ILogger<HttpClientTraceIdDemoController> logger)
    {
        _demoHttpClient = demoHttpClient;
        _logger = logger;
    }

    [HttpGet("InitExtensionTestTraceIdHttpClient")]
    public async Task InitTestTraceIdHttpClient()
    {
        await _demoHttpClient.SendViaExtensionAsync();
    }

    [HttpGet("InitHandlerTestTraceIdHttpClient")]
    public async Task InitHandlerTestTraceIdHttpClient()
    {
        await _demoHttpClient.SendViaHandlerAsync();
    }

    [HttpGet("TestTraceIdHttpClient")]
    public async Task TestTraceIdHttpClient()
    {
        Request.Headers.TryGetValue(LoggerConstants.LoggerHeaders.TracerIdHeader, out var traceIdHeaderValue);
        _logger.LogInformation("TraceId: {TraceId}, TraceIdHeaderValue: {0}", traceIdHeaderValue);
    }
}
