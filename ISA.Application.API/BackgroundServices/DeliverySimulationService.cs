using ISA.Application.API.SignalRServer;
using ISA.Core.Domain.UseCases.Company;
using PolylineEncoder.Net.Models;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using Microsoft.AspNetCore.SignalR;
using System.Text;
using Newtonsoft.Json;
using ISA.Core.Domain.UseCases.Contract;

namespace ISA.Application.API.BackgroundServices;

public class DeliverySimulationService : BackgroundService
{


    private readonly ILogger<DeliverySimulationService> _logger;
    private IServiceScopeFactory _serviceScopeFactory;
    private readonly IHubContext<SignalRHub> _hubContext;



    public DeliverySimulationService(ILogger<DeliverySimulationService> logger, IServiceScopeFactory serviceScopeFactory, Microsoft.AspNetCore.SignalR.IHubContext<SignalRHub> hubContext)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _hubContext = hubContext;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var rabbitmqHost = "172.206.250.97";
                var rabbitmqQueue = "Cooridantes";
                var rabbitmqUser = "guest";
                var rabbitmqPassword = "guest";

                var factory = new ConnectionFactory()
                {
                    HostName = rabbitmqHost,
                    UserName = rabbitmqUser,
                    Password = rabbitmqPassword,
                    DispatchConsumersAsync = true
                };

                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: rabbitmqQueue,
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var consumer = new AsyncEventingBasicConsumer(channel);

                    consumer.Received += async (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var messageJson = Encoding.UTF8.GetString(body);
                        Console.WriteLine($"Received message: {messageJson}");
                        _logger.LogInformation(messageJson);

                        try
                        {
                            var messageObject = JsonConvert.DeserializeObject<Message>(messageJson);

                            if (messageObject != null)
                            {

                                _logger.LogInformation($"Parsed message with companyId: {messageObject.companyId}");
                                await ProcessMessageAsync(messageObject);
                            }
                        }
                        catch (JsonException ex)
                        {
                            _logger.LogError($"Error deserializing message: {ex.Message}");
                        }

                        await Task.Yield();
                    };


                    channel.BasicConsume(queue: rabbitmqQueue,
                                         autoAck: true,
                                         consumer: consumer);

                    Console.WriteLine("Waiting for messages. To exit, press Ctrl+C");

                    await Task.Delay(Timeout.Infinite, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }



    private async Task ProcessMessageAsync(Message message)
    {

        using (IServiceScope scope = _serviceScopeFactory.CreateScope())
        { 
            var companyService = scope.ServiceProvider.GetRequiredService<CompanyService>();

            var admins = await companyService.GetCompanyAdmins(Guid.Parse(message.companyId));
            var adminIds = await companyService.GetCompanyAdmins(Guid.Parse(message.companyId));

            if(message.status == "done")
            {
                var contractService = scope.ServiceProvider.GetRequiredService<ContractService>();
                await contractService.TransferEquipment(Guid.Parse(message.companyId));
            }

            foreach (var adminId in adminIds)
            {
                var connectionId = ConnectionMapping.GetConnectionId(adminId);
                if (connectionId != null)
                {
                    await SendMessageToSpecificClient(connectionId, message.coordinate);
                }
            }
        }

        Console.WriteLine($"Received and processed: {message}");
    }

    public async Task SendMessageToSpecificClient(string connectionId, IGeoCoordinate message)
    {
        var messageToString = message.ToString();
        if (!string.IsNullOrEmpty(connectionId))
        {
            await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
        }
    }

}
