using MassTransit;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(DeliverySimulator.Startup))]

namespace DeliverySimulator
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            
            builder.Services.AddTransient<IEventBus, EventBus>();
            builder.Services.AddTransient<SendToMessageBus>();
        }
    }
}
