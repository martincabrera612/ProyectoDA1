namespace BusinessLogic.Exceptions;

public class BusinessLogicException : Exception
{
    public BusinessLogicException(string message) : base(message)
    {
    }
    public BusinessLogicException() : base("A business logic exception occurred.")
    {
    }
}

