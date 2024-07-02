using MassTransit;
using TraceId.Demo.Dtos.MassTransit.Requests;
using TraceId.Demo.Dtos.MassTransit.Responses;

namespace TraceId.Demo.Services;

public class MassTransitService
{
    private readonly IRequestClient<RequestClientDemoRequest> _requestClient;

    public MassTransitService(IRequestClient<RequestClientDemoRequest> requestClient)
    {
        _requestClient = requestClient;
    }

    public async Task<string> GetClientRequestMessage(RequestClientDemoRequest request)
    {
        var response = await _requestClient.GetResponse<RequestClientDemoResponse>(request);

        return response.Message.Message;
    }
}
