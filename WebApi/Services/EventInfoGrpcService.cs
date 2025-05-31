using Grpc.Net.Client;

namespace WebApi.Services;

public class EventInfoGrpcService
{
    private readonly EventInfoManager.EventInfoManagerClient _client;

    public EventInfoGrpcService(IConfiguration config)
    {
        var grpcUrl = config["Grpc:EventInfoUrl"] ?? "https://localhost:7018";

        var channel = GrpcChannel.ForAddress(grpcUrl);
        _client = new EventInfoManager.EventInfoManagerClient(channel);
    }

    public async Task<EventInfoRes> UpdateEventInfoAsync(int eventId, int totalTickets)
    {
        var request = new EventInfoUpdateReq
        {
            EventId = eventId,
            TotalTickets = totalTickets
        };

        var result = await _client.UpdateEventInfoAsync(request);
        return result;
    }

    public async Task<EventInfoRes> DeleteEventInfoAsync(int eventId)
    {
        var request = new EventInfoDeleteReq
        {
            EventId = eventId
        };

        return await _client.DeleteEventInfoAsync(request);
    }
}
