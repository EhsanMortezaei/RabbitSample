using System.Text.Json;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NotificationManagement.Infrastructure.Messaging;

public class NotificationConsumer
{
    private readonly ConnectionFactory _factory;

    public NotificationConsumer()
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

    public void StartConsuming(string queue = "notification-queue", string exchange = "payment.completed")
    {
        var connection = _factory.CreateConnection();
        var channel = connection.CreateModel();

        channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false);
        channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Fanout, durable: true);
        channel.QueueBind(queue: queue, exchange: exchange, routingKey: "");

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var json = Encoding.UTF8.GetString(ea.Body.ToArray());
            var payment = JsonSerializer.Deserialize<PaymentMessage>(json);

            Console.WriteLine($" پرداخت موفق برای سفارش {payment.OrderId} به مبلغ {payment.Amount} در {payment.PaidAt}");
        };

        channel.BasicConsume(queue: queue, autoAck: true, consumer: consumer);
    }
}

public class PaymentMessage
{
    public int OrderId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaidAt { get; set; }
    public bool IsSuccessful { get; set; }
}
