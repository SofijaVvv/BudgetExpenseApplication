using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseSystem.Model.Dto.Response;

public class UserResponse
{
	public int Id { get; set; }
	public string Email { get; set; }
	public TokenResponse Token { get; set; }
	public RoleResponse Role { get; set; }
}
