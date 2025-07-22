namespace PaymentManagement.Domain.Entities;

public sealed class Payment
{
    public int Id { get; private set; }
    public int OrderId { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime PaidAt { get; private set; }
    public bool IsSuccessful { get; private set; }

    private Payment() { }

    public Payment(Guid orderId, decimal amount)
    {
        if (orderId == Guid.Empty)
            throw new ArgumentException("شناسه سفارش معتبر نیست.");
        if (amount <= 0)
            throw new ArgumentException("مبلغ پرداخت باید مثبت باشد.");

        Amount = amount;
        PaidAt = DateTime.UtcNow;

        IsSuccessful = true;
    }
}
