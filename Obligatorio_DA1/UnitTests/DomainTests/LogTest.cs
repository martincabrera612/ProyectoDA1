using BusinessLogic.Validators;
using Domain;
using Domain.Enums;

namespace UnitTests.DomainTests;

[TestClass]
public class LogTest
{
    private Log _log;
    
    [TestInitialize]
    public void Init()
    {
        _log = new Log();
    }
    
    [TestMethod]
    public void ValidateEventNotNull()
    {
        _log.EventType = EventType.UserLogin;
        Assert.IsNotNull(_log.EventType);
    }
    
    [TestMethod]
    public void ValidateUserNotNull()
    {
        _log.User = new User();
        Assert.IsNotNull(_log.User);
    }
    
    [TestMethod]
    public void ValidateTimestampNotNull()
    {
        Assert.IsNotNull(_log.TimeStamp);
    }
    
    [TestMethod]
    public void ValidateUuidGeneration()
    {
        Assert.IsFalse(string.IsNullOrEmpty(_log.Id));
        
        UuidValidator.IsValidUuid(_log.Id);
    }
}