using BusinessLogic.Controllers;
using BusinessLogic.Exceptions;
using Domain;
using Persistence;
using Moq;

namespace UnitTests.BusinessLogicTests;

[TestClass]
public class UserControllerTest
{
    private User aUser;
    private Mock<IRepository<User>> mockUserRepository;
    private UserController _userController;
    private string _passwordConfirmation = "AtestPassword123%";

    [TestInitialize]
    public void SetUp()
    {
        aUser = new User()
        {
            Name = "John",
            Surname = "Doe",
            Email = "johndoe@gmail.com",
            Password = "AtestPassword123%"
        };
        
        mockUserRepository = new Mock<IRepository<User>>();

        _userController = new UserController(mockUserRepository.Object);
    }
    
    [TestMethod]
    public void TestUserControllerRegister()
    {
        mockUserRepository.Setup(repo => repo.Add(aUser)).Returns(aUser);
        mockUserRepository.Setup(repo => repo.FindAll()).Returns(new List<User>());
        var response = _userController.Register(aUser, _passwordConfirmation);
        Assert.AreEqual(response, "User correctly registered.");
        mockUserRepository.Verify(repo => repo.Add(aUser), Times.Once);
    }

    [TestMethod]
    public void TestAdminUser()
    {
        mockUserRepository.Setup(repo => repo.FindAll()).Returns(new List<User>());
        _userController.Register(aUser, _passwordConfirmation);
        Assert.IsTrue(aUser.IsAdmin);
    }

    [TestMethod]
    public void TestNotAdminUser()
    {
        mockUserRepository.Setup(repo => repo.FindAll()).Returns(new List<User> { new User() });
        _userController.Register(aUser, _passwordConfirmation);
        Assert.IsFalse(aUser.IsAdmin);
    }

    [TestMethod]
    public void TestAuthenticateUser()
    {
        const string existingEmail = "existing@example.com";
        const string correctPassword = "correctPassword";
        const string incorrectPassword = "incorrectPassword";
        var existingUser = new User { Email = existingEmail, Password = correctPassword };
        
        mockUserRepository.Setup(repo => repo.Find(It.IsAny<Func<User, bool>>()))
            .Returns((Func<User, bool> predicate) => predicate(existingUser) ? existingUser : null);
        
        var authenticatedUser = _userController.AuthenticateUser(existingEmail, correctPassword);
        Assert.AreEqual(existingUser, authenticatedUser);
        
        Assert.ThrowsException<BusinessLogicException>(() => _userController.AuthenticateUser(existingEmail, incorrectPassword));
        
        mockUserRepository.Setup(repo => repo.Find(It.IsAny<Func<User, bool>>())).Returns((User?)null);
        Assert.ThrowsException<BusinessLogicException>(() => _userController.AuthenticateUser("nonexistent@example.com", "somePassword"));
    }
    
    [TestMethod]
    public void TestIsEmailInUse()
    {
        const string existingEmail = "existing@example.com";
        const string nonExistingEmail = "nonexistent@example.com";
        var existingUser = new User { Email = existingEmail };
    
        mockUserRepository.Setup(repo => repo.Find(It.IsAny<Func<User, bool>>()))
            .Returns((Func<User, bool> predicate) => 
                predicate(existingUser) ? existingUser : null);

        var existingEmailResult = _userController.IsEmailInUse(existingEmail);
        var nonExistingEmailResult = _userController.IsEmailInUse(nonExistingEmail);

        Assert.IsTrue(existingEmailResult);
        Assert.IsFalse(nonExistingEmailResult);
    }

}