using System.Threading.Tasks;

namespace DeliverySimulator;

public interface IEventBus
{
    Task PublishAsync<T>(T message) where T:class ;
}
