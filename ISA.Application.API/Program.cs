
using ISA.Application.API.Mappings;
using ISA.Application.API.Startup;
using ISA.Application.API.Startup.DBConfiguration;
using ISA.Application.API.Startup.DI;
using ISA.Core.Domain.BackgroundTasks;
using ISA.Core.Domain.UseCases.User;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.ConfigureAuth();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.ConfigureSwagger(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Configuration.AddEnvironmentVariables()
                     .AddUserSecrets(Assembly.GetExecutingAssembly(), true);


var connectionString = builder.Configuration.GetConnectionString("ISADB");

builder.Services.AddDbConfig(connectionString);
builder.Services.AddDICconfig();
builder.Services.AddHttpClientConfig(builder);
builder.Services.AddIdentityConfig();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.MapperConfig();
builder.Services.AddHostedService<ReservationOverdueService>();

/*builder.Services.AddMassTransit(x =>
{
    // Configure RabbitMQ with username and password
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("amqp://localhost:5672", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        // Register your consumer
        cfg.ReceiveEndpoint("Coordinates", e =>
        {
            e.ConfigureConsumer<MyMessageConsumer>(context);
        });
    });
});*/

builder.Services.AddHostedService<DeliverySimulatorService>();
/*builder.Services.AddSingleton(provider => provider.GetRequiredService<IBusControl>());
builder.Services.AddSingleton(provider => provider.GetRequiredService<IBus>());*/





var app = builder.Build();

//check for systemadmin

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var userService = serviceProvider.GetRequiredService<UserService>();
    await userService.CheckForSystemAdmin();
}

/*var sc = app.Services.CreateScope();
var servicePr = sc.ServiceProvider;
var busControl = servicePr.GetRequiredService<IBusControl>();*/


//busControl.Start();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
