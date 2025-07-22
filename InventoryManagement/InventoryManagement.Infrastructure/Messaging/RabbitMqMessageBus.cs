using InventoryManagement.Application.Interface;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace InventoryManagement.Infrastructure.Messaging
{
    public class RabbitMqMessageBus : IMessageBus
    {
        private readonly ConnectionFactory _factory;

        public RabbitMqMessageBus()
        {
            _factory = new ConnectionFactory
            {
                Uri = new Uri("amqps://jqjgepts:qfybXjOjNpNL4sygzjhaNWtzJog5irdl@beaver.rmq.cloudamqp.com/jqjgepts\r\n"),
                Ssl = new SslOption
                {
                    Enabled = true,
                    ServerName = "beaver.rmq.cloudamqp.com"
                }
            };
        }

        public Task PublishAsync(string topic, object payload)
        {
            using var connection = _factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "order-queue",
                         durable: true,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null);

            channel.ExchangeDeclare(exchange: topic, type: ExchangeType.Fanout, durable: true);

            channel.QueueBind(queue: "order-queue", exchange: topic, routingKey: "");

            var message = JsonSerializer.Serialize(payload);
            var body = Encoding.UTF8.GetBytes(message);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: topic,
                                 routingKey: "",
                                 basicProperties: properties,
                                 body: body);

            Console.WriteLine($"[x] پیام به Exchange '{topic}' ارسال شد.");

            return Task.CompletedTask;
        }
    }
}