using Domain.Enums;

namespace Domain;

public class Payment
{
    public string Id { get; set; }
    public double Amount { get; set; }
    public PaymentStatus Status { get; set; }
    public DateTime PaymentDate { get; set; }

    public Payment(double amount)
    {
        Id = Guid.NewGuid().ToString();
        Amount = amount;
        Status = PaymentStatus.Reserved;
        PaymentDate = DateTime.Today;
    }
}