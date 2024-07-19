using Microsoft.AspNetCore.Mvc;
using TraceId.Demo.Constants;
using TraceId.Demo.MassTransit.Services;
using TraceId.Demo.Services;

namespace TraceId.Demo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MassTransitTraceIdDemoController : ControllerBase
{
    private readonly MassTransitService _massTransitService;
    private readonly TraceIdService _traceIdService;
    private readonly ILogger<MassTransitTraceIdDemoController> _logger;
    private readonly DemoHttpClient _demoHttpClient;

    public MassTransitTraceIdDemoController(MassTransitService massTransitService,
        TraceIdService traceIdService,
        ILogger<MassTransitTraceIdDemoController> logger,
        DemoHttpClient demoHttpClient)
    {
        _massTransitService = massTransitService;
        _traceIdService = traceIdService;
        _logger = logger;
        _demoHttpClient = demoHttpClient;
    }

    [HttpPost("SendRequestClientMessage")]
    public async Task<object> SendRequestClientMessage([FromBody] string message)
    {
        _logger.LogInformation("Before sending into queue TraceId: {TraceId}");

        var result = await _massTransitService.GetClientRequestMessage(new Dtos.MassTransit.Requests.RequestClientDemoRequest
        {
            Message = message
        });

        _logger.LogInformation("After sending into queue TraceId: {TraceId)");

        return new
        {
            MessageTraceId = result,
            ServiceTraceId = _traceIdService.TraceId,
        };
    }

    [HttpGet("TestTraceIdToDb")]
    public async Task TestTraceIdToDB()
    {
        _logger.LogInformation("TraceId: {TraceId}");
    }
}
