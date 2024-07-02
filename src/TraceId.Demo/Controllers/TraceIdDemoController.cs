using Microsoft.AspNetCore.Mvc;
using TraceId.Demo.MassTransit.Services;
using TraceId.Demo.Services;

namespace TraceId.Demo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TraceIdDemoController : ControllerBase
{
    private readonly MassTransitService _massTransitService;
    private readonly TraceIdService _traceIdService;

    public TraceIdDemoController(MassTransitService massTransitService,
        TraceIdService traceIdService)
    {
        _massTransitService = massTransitService;
        _traceIdService = traceIdService;
    }

    [HttpPost("SendRequestClientMessage")]
    public async Task<object> SendRequestClientMessage([FromBody] string message)
    {
        var result = await _massTransitService.GetClientRequestMessage(new Dtos.MassTransit.Requests.RequestClientDemoRequest
        {
            Message = message
        });

        return new
        {
            MessageTraceId = result,
            ServiceTraceId = _traceIdService.TraceId,
        };
    }

    //[HttpPost]
    //public async Task<string> SendConsumerMessage([FromBody] string s)
    //{
    //    return "ss";
    //}
}
