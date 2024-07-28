using Domain.Enums;

namespace Domain;

public class Booking
{
    public string Id { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public Status Status { get; set; }
    public User _user { get; set; }
    public Deposit _deposit { get; set; }
    public double Price { get; set; }
    public string? RejectionReason { get; set; }
    public Payment? payment { get; set; }

    public Booking()
    {
        Id = Guid.NewGuid().ToString();
    }
}