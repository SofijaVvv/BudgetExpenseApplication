namespace BudgetExpenseSystem.Model.Dto.Response;

public class AccountResponse
{
	public int Id { get; set; }

	public int UserId { get; set; }
	public decimal Balance { get; set; }

	public DateTime CreatedDate { get; set; }
}
