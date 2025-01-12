namespace BudgetExpenseSystem.Model.Dto.Requests;

public class UpdateTransactionRequest
{
	public int CategoryId { get; set; }

	public int BudgetId { get; set; }

	public decimal Amount { get; set; }
}
