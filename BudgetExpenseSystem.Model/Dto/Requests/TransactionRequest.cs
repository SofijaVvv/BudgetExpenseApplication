namespace BudgetExpenseSystem.Model.Dto.Requests;

public class TransactionRequest
{
	public int AccountId { get; set; }

	public int CategoryId { get; set; }

	public string Currency { get; set; }

	public int BudgetId { get; set; }

	public decimal Amount { get; set; }

	public DateOnly TransactionDate { get; set; }
}
