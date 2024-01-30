using PolylineEncoder.Net.Models;
using RabbitMQ.Client;

namespace DeliverySimualtorV2;

public class SendToMessageBus
{
    public static void Send(IGeoCoordinate cors)
    {
        try
        {
            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest", Port = 5672, VirtualHost = "/" };

            // Create a connection and open a channel, dispose them when done
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                // Declare a queue
                channel.QueueDeclare(queue: "hello",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                Console.WriteLine("Queue declared.");
            }

            // The connection is automatically closed when exiting the using block
            Console.WriteLine("Connection closed.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}
