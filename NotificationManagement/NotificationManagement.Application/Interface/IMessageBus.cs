namespace NotificationManagement.Application.Interface;

public interface IMessageBus
{
    Task PublishAsync(string topic, object payload);
}