namespace BudgetExpenseSystem.Model.Dto.Response;

public class LoginResponse
{
	public UserResponse User { get; set; }
	public string Token { get; set; }
}
