namespace BudgetExpenseSystem.Domain.Exceptions;

public class InsufficientFundsException : Exception
{
	public InsufficientFundsException(string message) : base(message)
	{
	}
}
