namespace OrderManagement.Domain.Entities;

public sealed class OrderItem
{
    public int Id { get; private set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal TotalPrice => Quantity * UnitPrice;

    private OrderItem() { }

    public OrderItem(Guid productId, string productName, int quantity, decimal unitPrice)
    {
        if (productId == Guid.Empty)
            throw new ArgumentException("شناسه محصول معتبر نیست.", nameof(productId));

        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("نام محصول نمی‌تواند خالی باشد.", nameof(productName));

        if (quantity <= 0)
            throw new ArgumentException("تعداد باید بزرگ‌تر از صفر باشد.", nameof(quantity));

        if (unitPrice <= 0)
            throw new ArgumentException("قیمت واحد باید بزرگ‌تر از صفر باشد.", nameof(unitPrice));

        ProductId = productId;  
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }
}
