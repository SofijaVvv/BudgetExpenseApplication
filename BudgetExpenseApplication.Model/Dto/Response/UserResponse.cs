using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseSystem.Model.Dto.Response;

public class UserResponse
{
	public int Id { get; set; }
	public string Email { get; set; }
	public string FullName { get; set; }
	public string Role { get; set; }
}
