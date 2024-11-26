namespace BudgetExpenseSystem.Domain.CustomExceptions;

public class InvalidIdException : Exception
{
    public InvalidIdException(string message) : base(message) { }
    public InvalidIdException(string message, Exception innerException) 
        : base(message, innerException) { }
}