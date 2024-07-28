using Domain.Enums;

namespace Domain;

public class Deposit
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Area Area { get; set; } 
    public Size Size { get; set; }
    public bool AirConditioning { get; set; }
    public List<Promotion> Promotions { get; set; }
    public List<Booking> Bookings { get; set; }
    public List<Availability> Availabilities { get; set; }

    public Deposit() 
    {
        Id = Guid.NewGuid().ToString();
        Promotions = new List<Promotion>();
        Availabilities = new List<Availability>();
        Bookings = new List<Booking>();
    }
} 