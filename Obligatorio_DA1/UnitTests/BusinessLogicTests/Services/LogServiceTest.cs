using BusinessLogic.Services;
using Domain;
using Moq;
using Persistence;

namespace UnitTests.BusinessLogicTests;

[TestClass]
public class LogServiceTest
{
    private Mock<IRepository<Log>> mockLogRepository;
    private LogService _logService;
    private User aUser;

    [TestInitialize]
    public void SetUp()
    {
        aUser = new User();
        mockLogRepository = new Mock<IRepository<Log>>();

        _logService = new LogService(mockLogRepository.Object);
    }
    
    [TestMethod]
    public void LogUserLogin()
    {
        mockLogRepository.Setup(x => x.Add(It.IsAny<Log>())); 
        _logService.LogLogin(aUser);
        
        mockLogRepository.Verify(x => x.Add(It.IsAny<Log>()), Times.Once);
    }
    
    [TestMethod]
    public void LogUserLogout()
    {
        mockLogRepository.Setup(x => x.Add(It.IsAny<Log>()));
        _logService.LogLogout(aUser);
        
        mockLogRepository.Verify(x => x.Add(It.IsAny<Log>()), Times.Once);
    }
    
    [TestMethod]
    public void LogUserCreatedReview()
    {
        mockLogRepository.Setup(x => x.Add(It.IsAny<Log>()));
        _logService.LogReview(aUser);
        
        mockLogRepository.Verify(x => x.Add(It.IsAny<Log>()), Times.Once);
    }
}