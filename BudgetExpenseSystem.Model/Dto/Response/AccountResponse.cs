using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseSystem.Model.Dto.Response;

public class AccountResponse
{
	public int Id { get; set; }

	public decimal Balance { get; set; }

	public DateOnly CreatedDate { get; set; }
}