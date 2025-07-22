using MediatR;

namespace OrderManagement.Application.CreateOrder;

public class CreateOrderCommand : IRequest<int>
{
    public string CustomerName { get; set; }
    public string CustomerPhone { get; set; }
    public List<OrderItemDto> Items { get; set; }
}

public class OrderItemDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
