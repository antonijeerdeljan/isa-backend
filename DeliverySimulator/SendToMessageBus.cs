using MassTransit.Configuration;
using Newtonsoft.Json;
using PolylineEncoder.Net.Models;
using RabbitMQ.Client;
using System;
using System.Text;

namespace DeliverySimulator;

public class SendToMessageBus
{
    public async void Send(Message cord)
    {
        try
        {
            //var bus = BusConfiguratior
            //await _eventBus.PublishAsync(cord);


            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest", Port = 5672, VirtualHost = "/" };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "Cooridantes",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string message = JsonConvert.SerializeObject(cord);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "Cooridantes",
                                     basicProperties: null,
                                     body: body);

                Console.WriteLine(" [x] Sent {0}", message);
            }


        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}
