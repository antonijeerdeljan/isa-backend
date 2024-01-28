using MassTransit.Configuration;
using Newtonsoft.Json;
using PolylineEncoder.Net.Models;
using RabbitMQ.Client;
using System;
using System.Text;

namespace DeliverySimulator;

public class SendToMessageBus
{
    private readonly IEventBus _eventBus;
    public SendToMessageBus(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async void Send(IGeoCoordinate cord)
    {
        try
        {
            //var bus = BusConfiguratior
            //await _eventBus.PublishAsync(cord);


            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest", Port = 5672, VirtualHost = "/" };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                // Declare a queue
                channel.QueueDeclare(queue: "Cooridantes",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                // Serialize the IGeoCoordinate object to JSON
                string message = JsonConvert.SerializeObject(cord);
                var body = Encoding.UTF8.GetBytes(message);

                // Publish the message to the queue
                channel.BasicPublish(exchange: "",
                                     routingKey: "Cooridantes",
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
