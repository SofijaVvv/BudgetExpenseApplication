namespace BudgetExpenseSystem.Model.Dto.Requests;

public class UpdateAccountRequest
{
	public decimal Balance { get; set; }
	public string Currency { get; set; }
}
