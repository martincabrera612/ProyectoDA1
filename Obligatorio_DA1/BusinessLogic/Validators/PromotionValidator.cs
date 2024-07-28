using System.Text.RegularExpressions;
using Domain;

namespace BusinessLogic.Validators;

public static class PromotionValidator
{
    private const int MaxLabelLength = 20;
    private const int MinPercentage = 5;
    private const int MaxPercentage = 75;
    private const string ValidLabelPattern = @"^[a-zA-Z0-9\s]+$";

    public static void IsValid(Promotion promotion)
    {
        ValidateNotNull(promotion);
        ValidateLabel(promotion);
        ValidatePercentage(promotion);
        ValidateValidity(promotion);
    }

    public static void ValidateNotNull(Promotion? promotion)
    {
        if (promotion == null)
        {
            throw new ArgumentNullException(nameof(promotion));
        }
    }

    public static void ValidateLabel(Promotion promotion)
    {
        if (string.IsNullOrWhiteSpace(promotion.Label))
        {
            throw new InvalidOperationException("The label is required.");
        }

        if (promotion.Label.Length > MaxLabelLength)
        {
            throw new InvalidOperationException($"The label cannot be more than {MaxLabelLength} characters.");
        }

        if (!Regex.IsMatch(promotion.Label, ValidLabelPattern))
        {
            throw new InvalidOperationException("The label format is invalid.");
        }
    }

    public static void ValidatePercentage(Promotion promotion)
    {
        if (promotion.Percentage < MinPercentage || promotion.Percentage > MaxPercentage)
        {
            throw new InvalidOperationException($"The percentage must be between {MinPercentage} and {MaxPercentage}.");
        }
    }

    public static void ValidateValidity(Promotion promotion)
    {
        if (promotion.From > promotion.To)
        {
            throw new InvalidOperationException("The 'from' date cannot be greater than the 'to' date.");
        }
    }
}