namespace BudgetExpenseSystem.Model.Dto.Requests;

public class AccountRequest
{
	public decimal Balance { get; set; }

	public int UserId { get; set; }

	public string Currency { get; set; }

}
