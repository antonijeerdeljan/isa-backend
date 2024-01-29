using DeliverySimulator.MessageAndEventBus;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(DeliverySimulator.Startup))]

namespace DeliverySimulator
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            
            builder.Services.AddTransient<MessageBus>();
        }
    }
}
