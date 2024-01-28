using MassTransit;
using System.Threading.Tasks;

namespace DeliverySimulator;

public class EventBus : IEventBus
{

    private  readonly IPublishEndpoint _endpoint;

    public EventBus(IPublishEndpoint endpoint)
    {
        _endpoint = endpoint;
    }

    public async Task PublishAsync<T>(T message) where T : class
    {
        await _endpoint.Publish(message);
    }



}
