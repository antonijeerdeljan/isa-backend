using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace DeliverySimulator.MessageAndEventBus;

public class MessageBus
{
    public async void Send(Message cord)
    {
        try
        {

            var host = Environment.GetEnvironmentVariable("host", EnvironmentVariableTarget.Process);
            var username = Environment.GetEnvironmentVariable("user", EnvironmentVariableTarget.Process);
            var password = Environment.GetEnvironmentVariable("password", EnvironmentVariableTarget.Process);
            var queueName = Environment.GetEnvironmentVariable("queueName", EnvironmentVariableTarget.Process);

            var factory = new ConnectionFactory() { HostName = host, UserName = username, Password = password, Port = 5672, VirtualHost = "/" };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string message = JsonConvert.SerializeObject(cord);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: queueName,
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
