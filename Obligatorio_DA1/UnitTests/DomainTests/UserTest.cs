using BusinessLogic.Validators;
using Domain;

namespace UnitTests.DomainTests;

[TestClass]
public class UserTest
{
    private User _user = null!;

    [TestInitialize]
    public void Init()
    {
        _user = new User();
    }

    [TestMethod]
    public void ValidateNameNullTest()
    {
        _user.Name = "Joe";
        Assert.IsNotNull(_user.Name);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ValidateNameLenghtTest()
        {
            _user.Name =
            "inting Lorem Ipsum is simply dummy text of the prand typesetting industry. Lorem Ipsum has been these";
        UserValidator.ValidateName(_user);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ValidateSurnameLenghtTest()
    {
        _user.Surname =
            "inting Lorem Ipsum is simply dummy text of the prand typesetting industry. Lorem Ipsum has been these";
        UserValidator.ValidateSurname(_user);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ValidateEmailFormatTest()
    {
        _user.Email = "notavalidemail";
        UserValidator.ValidateEmail(_user);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ValidatePasswordFormatTest()
    {
        _user.Password = "invalid";
        UserValidator.ValidatePassword(_user, "invalid");
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ValidatePasswordConfirmationTest()
    {
        _user.Password = "AtestPassword123%";
        UserValidator.ValidatePassword(_user, "invalid");
    }
    
    [TestMethod]
    public void ValidateUUIDGeneration()
    {
        Assert.IsFalse(string.IsNullOrEmpty(_user.Id));
        
        UuidValidator.IsValidUuid(_user.Id);
    }
}