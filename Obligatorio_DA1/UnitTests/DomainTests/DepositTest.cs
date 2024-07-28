using BusinessLogic.Validators;
using Domain;
using Domain.Enums;

namespace UnitTests.DomainTests;

[TestClass]
public class DepositTest
{
    private Deposit _deposit = null!;

    [TestInitialize]
    public void Init()
    {
        _deposit = new Deposit();
    }

    [TestMethod]
    public void ValidateAreaNullTest()
    {
        _deposit.Area = Area.A;
        Assert.IsNotNull(_deposit.Area);
    }
    
    [TestMethod]
    public void ValidateSizeNullTest()
    {
        _deposit.Size = Size.Medium;
        Assert.IsNotNull(_deposit.Size);
    }
    
    [TestMethod]
    public void ValidateAirConditioningNullTest()
    {
        _deposit.AirConditioning = false;
        Assert.IsNotNull(_deposit.AirConditioning);
    }
    
    [TestMethod]
    public void ValidatePromotionNotNullTest()
    {
        var promotion = new Promotion { Id = "a7b4e045-e569-4a72-a00d-0730b9c84356", Label = "Discount", Percentage = 10 };
        _deposit.Promotions = new List<Promotion> { promotion };
        
        Assert.IsNotNull(_deposit.Promotions);
    }
    
    [TestMethod]
    public void ValidateUUIDGeneration()
    {
        Assert.IsFalse(string.IsNullOrEmpty(_deposit.Id));
        
        UuidValidator.IsValidUuid(_deposit.Id);
    }
}