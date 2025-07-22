namespace OrderManagement.Domain.Interface;

public interface IOrderRepository
{
    Task SaveAsync(Entities.Order order, CancellationToken cancellationToken);
}
