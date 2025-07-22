using MediatR;
using OrderManagement.Application.Interface;
using OrderManagement.Domain.Interface;

namespace OrderManagement.Application.CreateOrder;

public sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
{
    private readonly IOrderRepository _repository;
    private readonly IMessageBus _messageBus;

    public CreateOrderCommandHandler(IOrderRepository repository, IMessageBus messageBus)
    {
        _repository = repository;
        _messageBus = messageBus;
    }

    public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Domain.Entities.Order(request.CustomerName, request.CustomerPhone);

        foreach (var item in request.Items)
            order.AddItem(item.ProductId, item.ProductName, item.Quantity, item.UnitPrice);

        await _repository.SaveAsync(order, cancellationToken);

        await _messageBus.PublishAsync("order.created", new
        {
            orderId = order.Id,
            customer = order.CustomerName,
            phone = order.CustomerPhone,
            total = order.TotalAmount,
            items = order.Items.Select(i => new
            {
                i.Id,
                i.ProductName,
                i.Quantity,
                i.UnitPrice
            })
        });

        return order.Id;
    }
}