using BusinessLogic;
using BusinessLogic.Controllers;
using BusinessLogic.Exceptions;
using Domain;
using Persistence;
using Moq;

namespace UnitTests.BusinessLogicTests;

[TestClass]
public class PromotionControllerTest
{
    private Promotion aPromotion;
    private Mock<IRepository<Promotion>> mockPromotionRepository;
    private Mock<SessionLogic> mockSessionLogic;
    private PromotionController _promotionController;

    [TestInitialize]
    public void SetUp()
    {
        aPromotion = new Promotion()
        {
            Label = "Christmas Sale",
            Percentage = 70,
            From = new DateTime(2024, 12, 10),
            To = new DateTime(2024, 12, 25)
        };
        mockPromotionRepository = new Mock<IRepository<Promotion>>();
        mockSessionLogic = new Mock<SessionLogic>();
        
        _promotionController = new PromotionController(mockPromotionRepository.Object, mockSessionLogic.Object);
    }

    [TestMethod]
    public void TestAdminCanCreatePromotion()
    {
        mockSessionLogic.Setup(session => session.IsAdmin()).Returns(true);
        var result = _promotionController.Create(aPromotion);
        
        Assert.AreEqual("Promotion created successfully", result);
    }
    
    
    [TestMethod]
    public void TestNonAdminCannotCreatePromotion()
    {
        mockSessionLogic.Setup(session => session.IsAdmin()).Returns(false);
        var result = _promotionController.Create(aPromotion);
        
        Assert.AreEqual("You must be an admin to create a promotion.", result);
    }
    
    [TestMethod]
    public void TestAdminCanDeletePromotion()
    {
        const string promotionId = "53fcbdbe-c46a-4c37-9408-17c795d3b92b";
        mockPromotionRepository.Setup(repo => repo.Delete(promotionId));
        mockSessionLogic.Setup(session => session.IsAdmin()).Returns(true);
        
        var result = _promotionController.Delete(promotionId);
        
        Assert.AreEqual("Promotion deleted successfully", result);
    }
    
    [TestMethod]
    public void TestNonAdminCannotDeletePromotion()
    {
        const string promotionId = "53fcbdbe-c46a-4c37-9408-17c795d3b92b";
        mockSessionLogic.Setup(session => session.IsAdmin()).Returns(false);
        
        var result = _promotionController.Delete(promotionId);
        
        Assert.AreEqual("You must be an admin to delete a promotion.", result);
    }
    
    [TestMethod]
    public void TestPromotionControllerDeleteFailure()
    {
        const string invalidPromotionId = "2f4b47bc-602d-406d-bc3f-e42e230b6610 ";
        mockPromotionRepository.Setup(repo => repo.Delete(invalidPromotionId))
            .Throws(new BusinessLogicException());
        mockSessionLogic.Setup(session => session.IsAdmin()).Returns(true);
        
        var result = _promotionController.Delete(invalidPromotionId);
        
        Assert.AreEqual("A business logic exception occurred.", result);
    }
    
    [TestMethod]
    public void TestAdminCanUpdatePromotion()
    {
        mockPromotionRepository.Setup(repo => repo.Update(aPromotion)).Returns(aPromotion);
        mockSessionLogic.Setup(session => session.IsAdmin()).Returns(true);
        var response = _promotionController.Update(aPromotion);

        Assert.AreEqual(response, "Promotion updated successfully");
        mockPromotionRepository.Verify(repo => repo.Update(aPromotion), Times.Once);
    }
    
    [TestMethod]
    public void TestNonAdminCannotUpdatePromotion()
    {
        mockSessionLogic.Setup(session => session.IsAdmin()).Returns(false);
        var response = _promotionController.Update(aPromotion);

        Assert.AreEqual(response, "You must be an admin to update a promotion.");
    }
    
    [TestMethod]
    public void TestPromotionControllerUpdateFailure()
    {
        mockSessionLogic.Setup(session => session.IsAdmin()).Returns(true);
        mockPromotionRepository.Setup(repo => repo.Update(aPromotion))
            .Throws(new BusinessLogicException("Failed to update promotion in repository."));
    
        var result = _promotionController.Update(aPromotion);
    
        Assert.AreEqual("Failed to update promotion in repository.", result);
    }
}