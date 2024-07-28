using BusinessLogic;
using BusinessLogic.Controllers;
using Domain;
using Domain.Enums;
using Persistence;
using Moq;

namespace UnitTests.BusinessLogicTests;

[TestClass]
public class DepositControllerTest
{
    private Deposit aDeposit;
    private Mock<IRepository<Deposit>> mockDepositRepository;
    private Mock<SessionLogic> mockSessionLogic;
    private DepositController _depositController;

    [TestInitialize]
    public void SetUp()
    {
        aDeposit = new Deposit()
        {
            Id = "a7b4e045-e569-4a72-a00d-0730b9c84356",
            Size = Size.Medium,
            Area = Area.A,
            AirConditioning = false
        };
        mockDepositRepository = new Mock<IRepository<Deposit>>();
        mockSessionLogic = new Mock<SessionLogic>();

        _depositController = new DepositController(mockDepositRepository.Object, mockSessionLogic.Object);
    }

    [TestMethod]
    public void TestAdminCanCreateDeposit()
    {
        mockSessionLogic.Setup(session => session.IsAdmin()).Returns(true);
        var result = _depositController.Create(aDeposit);
        
        Assert.AreEqual("Deposit created successfully", result);
    }
    
    [TestMethod]
    public void TestNonAdminCannotCreateDeposit()
    {
        mockSessionLogic.Setup(session => session.IsAdmin()).Returns(false);
        var result = _depositController.Create(aDeposit);
        
        Assert.AreEqual("You must be an admin to create a deposit.", result);
    }
    
    [TestMethod]
    public void TestAdminCanDeleteDeposit()
    {
        const string depositId = "53fcbdbe-c46a-4c37-9408-17c795d3b92b";
        mockDepositRepository.Setup(repo => repo.Delete(depositId));
        mockSessionLogic.Setup(session => session.IsAdmin()).Returns(true);
        
        var result = _depositController.Delete(depositId);
        
        Assert.AreEqual("Deposit deleted successfully", result);
    }
    
    [TestMethod]
    public void TestNonAdminCannotDeleteDeposit()
    {
        const string depositId = "53fcbdbe-c46a-4c37-9408-17c795d3b92b";
        mockSessionLogic.Setup(session => session.IsAdmin()).Returns(false);
        
        var result = _depositController.Delete(depositId);
        
        Assert.AreEqual("You must be an admin to delete a deposit.", result);
    }
    
    [TestMethod]
    public void TestAdminCanUpdateDeposit()
    {
        mockSessionLogic.Setup(session => session.IsAdmin()).Returns(true);
        mockDepositRepository.Setup(repo => repo.Update(aDeposit));
        
        var result = _depositController.Update(aDeposit);
        
        Assert.AreEqual("Deposit created successfully", result);
    }
    
    [TestMethod]
    public void TestNonAdminCannotUpdateDeposit()
    {
        mockSessionLogic.Setup(session => session.IsAdmin()).Returns(false);
        
        var result = _depositController.Update(aDeposit);
        
        Assert.AreEqual("You must be an admin to update a deposit.", result);
    }

}