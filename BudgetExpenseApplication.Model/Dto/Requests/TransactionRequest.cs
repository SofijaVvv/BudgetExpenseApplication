namespace BudgetExpenseSystem.Model.Dto.Requests;

public class TransactionRequest
{

	public string Currency { get; set; }


	public int? BudgetId { get; set; }

	public decimal Amount { get; set; }
	public string TransactionType { get; set; }

}
