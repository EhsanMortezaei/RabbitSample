namespace InventoryManagement.Domain.Entities;

public sealed class Inventory
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public int Stock { get; private set; }

    private Inventory()
    {
    }

    public Inventory(int productId, string name, int stock)
    {
        if (productId <= 0)
            throw new ArgumentException("شناسه محصول معتبر نیست.", nameof(productId));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("نام محصول نمی‌تواند خالی باشد.", nameof(name));

        if (stock < 0)
            throw new ArgumentException("موجودی نمی‌تواند منفی باشد.", nameof(stock));

        Name = name;
        Stock = stock;
    }

    public void DecreaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("مقدار باید بزرگ‌تر از صفر باشد.");

        if (Stock < quantity)
            throw new InvalidOperationException("موجودی کافی نیست.");

        Stock -= quantity;
    }
}
