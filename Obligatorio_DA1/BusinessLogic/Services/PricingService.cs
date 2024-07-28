using Domain;
using Domain.Enums;

namespace BusinessLogic.Services;

public class PricingService
{
    
    public Deposit _deposit { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    

    public PricingService(Deposit deposit, DateTime from, DateTime to)
    {
        _deposit = deposit;
        From = from;
        To = to;
    }

    public PricingService()
    {
        
    }

    public double CalculatePrice()
    {
        double precio = (((SizeCost() + airConditioningCost())* DurationCost()) *PromotionCost());
        return precio;
    }

    public int SizeCost()
    {
        int sizeCost = 0;
        TimeSpan interval = To - From;
        if (_deposit.Size == Size.Small)
        {
            sizeCost += 50;
        }
        else if (_deposit.Size == Size.Medium)
        {
            sizeCost += 75;
        }
        else
        {
            sizeCost += 100;
        }

        return (sizeCost*interval.Days);
    }
    
    public double DurationCost()
    {
        double discount = 0;
        TimeSpan interval = To - From;
        if (interval.Days >= 7 && interval.Days <= 14 )
        {
            discount = 0.05;
        }
        else if (interval.Days > 14)
        {
            discount = 0.1;
        }
        return 1-discount ;
    }

    public int airConditioningCost()
    {
        int cost = 0;
        TimeSpan interval = To - From;
        if (_deposit.AirConditioning)
        {
            cost = interval.Days * 20;
        }

        return cost;
    }

    public double PromotionCost()
    {
        double discount = 1;
        var _promotion = _deposit.Promotions.FirstOrDefault(p => p.From <= From && p.To>= To);
        if (_promotion != null)
        {
            discount -= (_promotion.Percentage / 100.0);
        }
        return discount;
    }
    
}
