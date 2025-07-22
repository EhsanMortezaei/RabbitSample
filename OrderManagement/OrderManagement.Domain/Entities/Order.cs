using Order.Common;
using System.Text.RegularExpressions;

namespace OrderManagement.Domain.Entities;

public sealed class Order
{
    public int Id { get; private set; }
    public DateTime OrderDate { get; private set; }
    public OrderStatus Status { get; private set; }
    public string CustomerName { get; private set; }
    public string CustomerPhone { get; private set; }

    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    public decimal TotalAmount => _items.Sum(i => i.TotalPrice);

    private Order() { }

    public Order(string customerName, string customerPhone)
    {
        if (string.IsNullOrWhiteSpace(customerName))
            throw new ArgumentException("نام مشتری نمی‌تواند خالی باشد.", nameof(customerName));

        if (!IsValidPhoneNumber(customerPhone))
            throw new ArgumentException("شماره تلفن معتبر نیست.", nameof(customerPhone));

        OrderDate = DateTime.UtcNow;
        Status = OrderStatus.Pending;
        CustomerName = customerName;
        CustomerPhone = customerPhone;
    }

    public void AddItem(Guid productId, string productName, int quantity, decimal unitPrice)
    {
        if (productId == Guid.Empty)
            throw new ArgumentException("شناسه محصول معتبر نیست.", nameof(productId));

        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("نام محصول نمی‌تواند خالی باشد.", nameof(productName));

        if (quantity <= 0)
            throw new ArgumentException("تعداد باید بزرگ‌تر از صفر باشد.", nameof(quantity));

        if (unitPrice <= 0)
            throw new ArgumentException("قیمت واحد باید بزرگ‌تر از صفر باشد.", nameof(unitPrice));

        var item = new OrderItem(productId, productName, quantity, unitPrice);
        _items.Add(item);
    }

    public void ChangeStatus(OrderStatus newStatus)
    {
        if (Status != newStatus)
        {
            Status = newStatus;
        }
    }

    private bool IsValidPhoneNumber(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return false;

        var regex = new Regex(@"^09\d{9}$");
        return regex.IsMatch(phone);
    }
}
