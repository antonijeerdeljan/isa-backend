using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;
using Microsoft.Extensions.Logging;


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
                var rabbitmqHost = "localhost"; // Replace with your RabbitMQ server hostname or IP address
                var rabbitmqQueue = "Cooridantes"; // Replace with your queue name
                var rabbitmqUser = "guest"; // Replace with your RabbitMQ username
                var rabbitmqPassword = "guest"; // Replace with your RabbitMQ password

                var factory = new ConnectionFactory()
                {
                    HostName = rabbitmqHost,
                    UserName = rabbitmqUser,
                    Password = rabbitmqPassword,
                    DispatchConsumersAsync = true // Enable asynchronous event handlers
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
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine($"Received message: {message}");
                        _logger.LogInformation(message);
                        await Task.Yield(); // To ensure asynchronous execution
                    };

                    channel.BasicConsume(queue: rabbitmqQueue,
                                         autoAck: true, // Set to true if you want to acknowledge messages automatically
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

