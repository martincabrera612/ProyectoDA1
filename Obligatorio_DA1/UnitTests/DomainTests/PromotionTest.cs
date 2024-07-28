using BusinessLogic.Validators;
using Domain;

namespace UnitTests.DomainTests;

[TestClass]
public class PromotionTest
{
    private Promotion _promotion = null!;

    [TestInitialize]
    public void Init()
    {
        _promotion = new Promotion();
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ValidateNotNullPromotion()
    {
        PromotionValidator.ValidateNotNull(null);
    }

    [TestMethod]
    public void ValidateLabelNullTest()
    {
        _promotion.Label = "Christmas Sale";
        Assert.IsNotNull(_promotion.Label);
    }
    
    [TestMethod]
    public void ValidatePercentageNullTest()
    {
        _promotion.Percentage = 70;
        Assert.IsNotNull(_promotion.Percentage);
    }
    
    [TestMethod]
    public void ValidateFromNullTest()
    {
        _promotion.From = new DateTime(2024, 12, 10);
        Assert.IsNotNull(_promotion.From);
    }
    
    [TestMethod]
    public void ValidateToNullTest()
    {
        _promotion.To = new DateTime(2024, 12, 25);
        Assert.IsNotNull(_promotion.To);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ValidateLabelLengthTest()
        {
            _promotion.Label =
            "Lorem Ipsum is simply dummy text";
        PromotionValidator.ValidateLabel(_promotion);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ValidateLabelIsAlphanumericTest()
    {
        _promotion.Label =
            "NonAlphanumeric@Label";
        PromotionValidator.ValidateLabel(_promotion);
    }
    
    [TestMethod]
    public void ValidateLabelCanHaveSpacesTest()
    {
        _promotion.Label =
            "Alphanumeric 1234";
        PromotionValidator.ValidateLabel(_promotion);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ValidateInvalidLabelFormatTest()
    {
        _promotion.Label = "Invalid@Label";
        PromotionValidator.ValidateLabel(_promotion);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ValidateEmptyLabelTest()
    {
        _promotion.Label = "";
        PromotionValidator.ValidateLabel(_promotion);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ValidateNegativePercentageTest()
    {
        _promotion.Percentage = -1;
        PromotionValidator.ValidatePercentage(_promotion);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ValidateMinimumPercentageTest()
    {
        _promotion.Percentage =
           4;
        PromotionValidator.ValidatePercentage(_promotion);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ValidateMaximumPercentageTest()
    {
        _promotion.Percentage =
            76;
        PromotionValidator.ValidatePercentage(_promotion);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ValidateToGreaterThanFromTest()
    {
        _promotion.From = new DateTime(2024, 12, 25);
        _promotion.To = new DateTime(2024, 12, 10);
        PromotionValidator.ValidateValidity(_promotion);
    }
    
    [TestMethod]
    public void ValidateUuidGeneration()
    {
        Assert.IsFalse(string.IsNullOrEmpty(_promotion.Id));
        
        UuidValidator.IsValidUuid(_promotion.Id);
    }
}