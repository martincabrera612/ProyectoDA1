using Domain;
using Domain.Enums;

namespace BusinessLogic.Validators;

public static class DepositValidator
{

    public static void IsValid(Deposit aDeposit)
    {
        ValidateName(aDeposit);
        
    }

    public static void ValidateName(Deposit aDeposit)
    {
        if (string.IsNullOrWhiteSpace(aDeposit.Name))
        {
            throw new InvalidOperationException("The Name cannot be Null or Empty");
        }

        if (!aDeposit.Name.All(char.IsLetter))
        {
            throw new InvalidOperationException("The Name must contain only letters");
        }
    }
}