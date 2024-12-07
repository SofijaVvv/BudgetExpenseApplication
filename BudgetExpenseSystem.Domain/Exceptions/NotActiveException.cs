namespace BudgetExpenseSystem.Domain.Exceptions;

public class NotActiveException : Exception
{
	public NotActiveException(string message) : base(message)
	{
	}
}
