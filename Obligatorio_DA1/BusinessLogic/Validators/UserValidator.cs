using System.Text.RegularExpressions;
using Domain;

namespace BusinessLogic.Validators;

public static class UserValidator
{
    private const string ValidEmailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
    private const string ValidPasswordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$#.,%])[A-Za-z\d@$#.,%]{8,}$";
    private static readonly char[] Symbols = { '@', '$', '#', '.', ',', '%' };
    public static bool IsValid(User aUser, string passwordConfirmation)
    {
        ValidateName(aUser);
        ValidateSurname(aUser);
        ValidateEmail(aUser);
        ValidatePassword(aUser, passwordConfirmation);
        return true;
    }

    public static void ValidateName(User aUser)
    {
        if (aUser.Name.Length > 100)
        {
            throw new InvalidOperationException("The name cannot be more than 100 characters.");
        }
    }

    public static void ValidateSurname(User aUser)
    {
        if (aUser.Surname.Length > 100)
        {
            throw new InvalidOperationException("The surname cannot be more than 100 characters.");
        }
    }

    public static void ValidateEmail(User aUser)
    {
        if (!IsValidEmail(aUser.Email))
        {
            throw new InvalidOperationException("The email format is invalid.");
        }
    }

    public static void ValidatePassword(User aUser, string passwordConfirmation)
    {
        if (!IsValidPassword(aUser.Password))
        {
            throw new InvalidOperationException("The password format is invalid.");
        }
        if (aUser.Password != passwordConfirmation)
        {
            throw new InvalidOperationException("The password and confirmation do not match.");
        }
    }

    private static bool IsValidEmail(string email)
    {
        return Regex.IsMatch(email, ValidEmailPattern);
    }

    private static bool IsValidPassword(string password)
    {
        if (password.Length < 8)
            return false;
        
        return Regex.IsMatch(password, ValidPasswordPattern) && ContainsSymbol(password);
    }

    private static bool ContainsSymbol(string password)
    {
        return Symbols.Any(password.Contains);
    }
}