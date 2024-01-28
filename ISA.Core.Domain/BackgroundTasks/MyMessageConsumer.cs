using MassTransit;
using PolylineEncoder.Net.Models;

namespace ISA.Core.Domain.BackgroundTasks;

public class MyMessageConsumer : IConsumer<IGeoCoordinate>
{
    

    public Task Consume(ConsumeContext<IGeoCoordinate> context)
    {
        throw new NotImplementedException();
    }
}
