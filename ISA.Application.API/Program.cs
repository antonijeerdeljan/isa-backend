
using ISA.Application.API.BackgroundServices;
using ISA.Application.API.Middleware;
using ISA.Application.API.SignalRServer;
using ISA.Application.API.Startup;
using ISA.Application.API.Startup.DBConfiguration;
using ISA.Application.API.Startup.DI;
using ISA.Core.Domain.UseCases.Delivery;
using ISA.Core.Domain.UseCases.User;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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
builder.Services.ConfigureAuth();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.MapperConfig();
builder.Services.AddHostedService<ReservationOverdueService>();
builder.Services.AddHostedService<DeliverySimulationService>();


builder.Services.AddSignalR();



var app = builder.Build();

//check for systemadmin

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var userService = serviceProvider.GetRequiredService<UserService>();
    var deliveryService = serviceProvider.GetRequiredService<DeliveryService>();

    await deliveryService.Delivery();
    await userService.CheckForSystemAdmin();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.MapHub<SignalRHub>("delivery");

app.Run();
