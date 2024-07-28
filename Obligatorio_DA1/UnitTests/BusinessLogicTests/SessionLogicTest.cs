using BusinessLogic;
using BusinessLogic.Controllers;
using BusinessLogic.Exceptions;
using BusinessLogic.Services;
using Domain;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace UnitTests.BusinessLogicTests;

[TestClass]
public class SessionLogicTest
{
    private Mock<UserController> userControllerMock;
    private Mock<LogService> logServiceMock; 
    private SessionLogic sessionLogic;
    private IMemoryCache cache;
    private User expectedUser;

    [TestInitialize]
    public void SetUp()
    {
        userControllerMock = new Mock<UserController>();
        logServiceMock = new Mock<LogService>();
        cache = new MemoryCache(new MemoryCacheOptions());
        
        sessionLogic = new SessionLogic(userControllerMock.Object, logServiceMock.Object, cache);

        expectedUser = new User
        {
            Email = "test@example.com",
            Password = "password",
            IsAdmin = false
        };

        userControllerMock.Setup(x => x.AuthenticateUser("test@example.com", "password"))
                          .Returns(expectedUser);
    }

    [TestMethod]
    public void LoginSuccessTest()
    {
        logServiceMock.Setup(x => x.LogLogin(expectedUser));
        sessionLogic.Login("test@example.com", "password");

        Assert.AreEqual(expectedUser, sessionLogic.CurrentUser);
    }

    [TestMethod]
    public void LoginUnsuccessfulTest()
    {
        userControllerMock.Setup(x => x.AuthenticateUser("test@example.com", "wrong_password"))
                          .Throws<BusinessLogicException>();

        Assert.ThrowsException<BusinessLogicException>(() => sessionLogic.Login("test@example.com", "wrong_password"));
    }

    [TestMethod]
    public void LogoutTest()
    {
        logServiceMock.Setup(x => x.LogLogout(expectedUser));
        sessionLogic.Logout();

        Assert.IsNull(sessionLogic.CurrentUser);
    }

    [TestMethod]
    public void IsLoggedTrueTest()
    {
        sessionLogic.CurrentUser = expectedUser;

        Assert.IsTrue(sessionLogic.IsLoggedIn());
    }

    [TestMethod]
    public void IsLogedFalseTest()
    {
        Assert.IsFalse(sessionLogic.IsLoggedIn());
    }

    [TestMethod]
    public void IsAdminTrueTest()
    {
        expectedUser.IsAdmin = true;
        sessionLogic.CurrentUser = expectedUser;

        Assert.IsTrue(sessionLogic.IsAdmin());
    }

    [TestMethod]
    public void IsAdminFalseTest()
    {
        expectedUser.IsAdmin = false;
        sessionLogic.CurrentUser = expectedUser;

        Assert.IsFalse(sessionLogic.IsAdmin());
    }

    [TestMethod]
    public void UserLoginLoggedEventTest()
    {
        bool eventRaised = false;
        sessionLogic.OnLogin += () => eventRaised = true;

        sessionLogic.Login("test@example.com", "password");

        Assert.IsTrue(eventRaised);
    }
}
