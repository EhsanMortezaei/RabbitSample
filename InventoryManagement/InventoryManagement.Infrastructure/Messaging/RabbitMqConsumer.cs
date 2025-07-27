using InventoryManagement.Application.Interface;
using InventoryManagement.Domain.Interface;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace InventoryManagement.Infrastructure.Messaging;

public class RabbitMqConsumer
{
    private readonly ConnectionFactory _factory;
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IMessageBus _messageBus;

    public RabbitMqConsumer(IInventoryRepository inventoryRepository, IMessageBus messageBus = null)
    {
        _inventoryRepository = inventoryRepository;
        _messageBus = messageBus;
    }

    public RabbitMqConsumer()
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

    public void StartConsuming(string queueName = "order-queue")
    {
        var connection = _factory.CreateConnection();
        var channel = connection.CreateModel();

        channel.QueueDeclare(queue: queueName,
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
            var order = System.Text.Json.JsonSerializer.Deserialize<OrderMessage>(message, options);

            foreach (var item in order.Items)
            {
                var inventory = _inventoryRepository.GetByProductName(item.ProductName);
                if (inventory == null)
                    throw new Exception($"Inventory not found for product: {item.ProductName}");

                inventory.DecreaseStock(item.Quantity);
            }
            _inventoryRepository.SaveAsync();


            _messageBus.PublishAsync("inventory.created", new
            {
                orderId = order.OrderId,
                total = order.Total
            });

            Console.WriteLine($"[x] پیام دریافت شد: {message}");

        };

        channel.BasicConsume(queue: queueName,
                             autoAck: true,
                             consumer: consumer);

    }
}

public class OrderMessage
{
    public int OrderId { get; set; }
    public string Customer { get; set; }
    public string Phone { get; set; }
    public int Total { get; set; }
    public List<OrderItem> Items { get; set; }
}

public sealed class OrderItem
{
    public int Id { get; set; }
    public string? ProductName { get; set; }
    public int Quantity { get; set; }
    public int UnitPrice { get; set; }
}
