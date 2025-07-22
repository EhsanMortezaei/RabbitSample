using OrderManagement.Domain.Interface;

namespace OrderManagement.Infrastructure.Commands.Orders;

public class OrderRepository : IOrderRepository
{
    private readonly OrderDbContext _orderDbContext;

    public OrderRepository(OrderDbContext orderDbContext)
    {
        _orderDbContext = orderDbContext;
    }

    public async Task SaveAsync(Domain.Entities.Order order, CancellationToken cancellationToken)
    {
        _orderDbContext.Add(order);
        await _orderDbContext.SaveChangesAsync();
    }
}
