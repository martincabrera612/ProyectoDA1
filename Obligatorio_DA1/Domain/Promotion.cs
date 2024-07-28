namespace Domain;

public class Promotion
{
    public string Id { get; set; }
    public string Label { get; set; }
    public int Percentage { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }

    public Promotion()
    {
        Id = Guid.NewGuid().ToString();
    }
    
}