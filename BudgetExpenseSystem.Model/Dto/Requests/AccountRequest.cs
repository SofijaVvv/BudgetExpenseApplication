namespace BudgetExpenseSystem.Model.Dto.Requests;

public class AccountRequest
{
	public decimal Balance { get; set; }

	public int UserId { get; set; }

	public DateOnly CreatedDate { get; set; }
}