using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ISA.Core.Domain.BackgroundTasks;

public class DeliverySimulatorService : BackgroundService
{
    private IConnection _connection;
    private IModel _channel;
    private string _queueName = "Coordinates";

    private readonly ILogger<DeliverySimulatorService> _logger;

    public DeliverySimulatorService(ILogger<DeliverySimulatorService> logger)
    {
        _logger = logger;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var rabbitmqHost = "localhost"; 
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

                    await Task.Delay(Timeout.Infinite, stoppingToken); // Keep the method running
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                // Handle the exception (log, etc.)
            }
        }
    }



    private async Task ProcessMessageAsync(string message)
    {
        // Implement your message processing logic here
        // Ensure all asynchronous operations within this method are awaited properly
        Console.WriteLine($"Received and processed: {message}");
    }
}

