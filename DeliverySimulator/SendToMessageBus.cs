using Newtonsoft.Json;
using PolylineEncoder.Net.Models;
using RabbitMQ.Client;
using System;
using System.Text;

namespace DeliverySimulator;

public static class SendToMessageBus
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

                // Serialize the IGeoCoordinate object to JSON
                string message = JsonConvert.SerializeObject(cors);
                var body = Encoding.UTF8.GetBytes(message);

                // Publish the message to the queue
                channel.BasicPublish(exchange: "",
                                     routingKey: "hello",
                                     basicProperties: null,
                                     body: body);

                Console.WriteLine(" [x] Sent {0}", message);
            }

            // The connection is automatically closed when exiting the using block
            //Console.WriteLine("Connection closed.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}
