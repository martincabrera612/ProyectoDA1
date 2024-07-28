using BusinessLogic.Services;
using Domain;
using Domain.Enums;
using DateTime = System.DateTime;

namespace UnitTests.DomainTests;

[TestClass]
public class PricingServiceTest
{

    [TestMethod]
    public void CalculatePriceSmallSizeNoAirConditioning()
    {
        List<Promotion> promotions = new List<Promotion>();
        Deposit deposit = new Deposit
        {
            AirConditioning = false, Area = Area.A, Id = "a7b4e045-e569-4a72-a00d-0730b9c84356",
            Promotions = promotions, Size = Size.Small
        };
        DateTime from = new DateTime();
        DateTime to = new DateTime().AddDays(1);
        PricingService pricingService = new PricingService(deposit,from,to);

        var price = pricingService.CalculatePrice();
        Assert.AreEqual(50,price);
    }

    [TestMethod]
    public void ConstructorInitializes()
    {
        Deposit deposit = new Deposit();
        DateTime from = new DateTime();
        DateTime to = new DateTime().AddDays(1);
        var pricingService = new PricingService(deposit, from, to);
        Assert.IsNotNull(pricingService._deposit);
        Assert.AreEqual(deposit, pricingService._deposit);
        Assert.AreEqual(from, pricingService.From);
        Assert.AreEqual(to, pricingService.To);
    }
    
    [TestMethod]
    public void CalculatePriceMediumSizeNoAirConditioning()
    {
        List<Promotion> promotions = new List<Promotion>();
        Deposit deposit = new Deposit
        {
            AirConditioning = false, Area = Area.A, Id = "a7b4e045-e569-4a72-a00d-0730b9c84356",
            Promotions = promotions, Size = Size.Medium
        };
        DateTime from = new DateTime();
        DateTime to = new DateTime().AddDays(1);
        PricingService pricingService = new PricingService(deposit,from,to);

        var price = pricingService.CalculatePrice();
        Assert.AreEqual(75,price);
    }
   
    [TestMethod]
    public void CalculatePriceBigSizeNoAirConditioning()
    {
        List<Promotion> promotions = new List<Promotion>();
        Deposit deposit = new Deposit
        {
            AirConditioning = false, Area = Area.A, Id = "a7b4e045-e569-4a72-a00d-0730b9c84356",
            Promotions = promotions, Size = Size.Big
        };
        DateTime from = new DateTime();
        DateTime to = new DateTime().AddDays(1);
        PricingService pricingService = new PricingService(deposit,from,to);

        var price = pricingService.CalculatePrice();
        Assert.AreEqual(100,price);
    }
    
    [TestMethod]
    public void DurationCostTest()
    {
        List<Promotion> promotions = new List<Promotion>();
        Deposit deposit = new Deposit
        {
            AirConditioning = false, Area = Area.A, Id = "a7b4e045-e569-4a72-a00d-0730b9c84356",
            Promotions = promotions, Size = Size.Big
        };
        DateTime from = new DateTime();
        DateTime to = new DateTime().AddDays(8);
        PricingService pricingService = new PricingService(deposit,from,to);

        var discount = pricingService.DurationCost();
        Assert.AreEqual(0.95,discount);
    }
    
    [TestMethod]
    public void NoPromotionCostTest()
    {
        List<Promotion> promotions = new List<Promotion>();
        Deposit deposit = new Deposit
        {
            AirConditioning = false, Area = Area.A, Id = "a7b4e045-e569-4a72-a00d-0730b9c84356",
            Promotions = promotions, Size = Size.Big
        };
        DateTime from = new DateTime();
        DateTime to = new DateTime().AddDays(8);
        PricingService pricingService = new PricingService(deposit,from,to);

        var discount = pricingService.PromotionCost();
        Assert.AreEqual(1,discount);
    }
    
    [TestMethod]
    public void PromotionCostTest()
    {
        Promotion promotion = new Promotion { From = new DateTime(2024, 12, 09), Id = "a7b4e045-e569-4a72-a00d-0730b9c84350", Label = "10%OFF", Percentage = 10, To = new DateTime(2024,12,31)};
        List<Promotion> listPromotions = new List<Promotion>();
        listPromotions.Add(promotion);
        DateTime from = new DateTime(2024, 12, 10);
        DateTime to = new DateTime(2024, 12, 20);
        
        Deposit deposit = new Deposit
        {
            AirConditioning = false, Area = Area.A, Id = "a7b4e045-e569-4a72-a00d-0730b9c84356",
            Promotions = listPromotions, Size = Size.Big
        };
        PricingService pricingService = new PricingService(deposit,from,to);

        var discount = pricingService.PromotionCost();
        Assert.AreEqual(0.9,discount);
    }
    
}