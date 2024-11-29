namespace BudgetExpenseSystem.Model.Dto.Response;

public class UserResponse
{
    public int Id { get; set; }
    
    public string Emaill { get; set; }
    
    public string PasswordHash { get; set; }
    
    public string PasswordSalt { get; set; }
    
    public int RoleId { get; set; }
}