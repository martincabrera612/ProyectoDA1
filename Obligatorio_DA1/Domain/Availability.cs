namespace Domain;

public class Availability
{
    public string Id { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    
    public Availability()
    {
        Id = Guid.NewGuid().ToString();
    }
}