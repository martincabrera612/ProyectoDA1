using Domain;

namespace BusinessLogic.Validators;

public class AvailabilityValidator
{
    public static void Validate(Availability aAvailability)
    {
        if (aAvailability.From == null)
        {
            throw new InvalidOperationException("The From date cannot be null");
        }

        if (aAvailability.To == null)
        {
            throw new InvalidOperationException("The To date cannot be null");
        }

        if (aAvailability.From > aAvailability.To)
        {
            throw new InvalidOperationException("The From date cannot be greater than the To date");
        }
    }
}