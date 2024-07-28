namespace Domain;

public class Review
{
    public string Id { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
    public User User { get; set; }
    public Booking Booking { get; set; }
    
    public Review()
    {
        Id = Guid.NewGuid().ToString();
    }
}