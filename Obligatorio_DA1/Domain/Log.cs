using Domain.Enums;

namespace Domain;

public class Log
{
    public string Id { get; set; }
    public EventType EventType { get; set; }
    public DateTime TimeStamp { get; set; }
    public User User { get; set; }
    
    public Log()
    {
        Id = Guid.NewGuid().ToString();
        TimeStamp = DateTime.Now;
    }
}