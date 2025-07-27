using InventoryManagement.Infrastructure.Messaging;
using PaymentManagement.Application.Interface;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace PaymentManagement.Infrastructure.Messaging;

public class PaymentConsumer
{
    private readonly ConnectionFactory _factory;
    private readonly IMessageBus _messageBus;

    public PaymentConsumer()
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

    public void StartConsuming(string queue = "payment-queue", string exchange = "order.created")
    {
        var connection = _factory.CreateConnection();
        var channel = connection.CreateModel();

        channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false);
        channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Fanout, durable: true);
        channel.QueueBind(queue: queue, exchange: exchange, routingKey: "");

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            var json = Encoding.UTF8.GetString(ea.Body.ToArray());
            var order = JsonSerializer.Deserialize<OrderMessage>(json);

            Console.WriteLine($" در حال پرداخت سفارش: {order.OrderId} - مبلغ: {order.Total}");

            await Task.Delay(1000);

            var paymentEvent = new
            {
                OrderId = order.OrderId,
                Amount = order.Total,
                PaidAt = DateTime.UtcNow,
                IsSuccessful = true
            };

            await _messageBus.PublishAsync("payment.completed", paymentEvent);
        };

        channel.BasicConsume(queue: queue, autoAck: true, consumer: consumer);
    }
}
