using Domain;
using Domain.Enums;
using Persistence;

namespace BusinessLogic.Services;

public class LogService
{
    private readonly IRepository<Log> _logRepository;
    private readonly SessionLogic _sessionLogic;
    
    public LogService()
    {
    }
    
    public LogService(IRepository<Log> aLogRepository)
    {
        _logRepository = aLogRepository;
    }
    
    public virtual void LogLogin(User aUser)
    {
        Log aLog = new Log();
        aLog.EventType = EventType.UserLogin;
        aLog.User = aUser;
        _logRepository.Add(aLog);
    }
    
    public virtual void LogLogout(User aUser)
    {
        Log aLog = new Log();
        aLog.EventType = EventType.UserLogout;
        aLog.User = aUser;
        _logRepository.Add(aLog);
    }

    public virtual void LogReview(User aUser)
    {
        Log aLog = new Log();
        aLog.EventType = EventType.UserReview;
        aLog.User = aUser;
        _logRepository.Add(aLog);
    }
}