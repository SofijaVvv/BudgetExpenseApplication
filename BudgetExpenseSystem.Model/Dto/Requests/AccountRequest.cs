namespace BudgetExpenseSystem.Model.Dto.Requests;

public class AccountRequest
{
    public int UserId { get; set; }
    
    public decimal Balance { get; set; }
    
    public DateOnly CreatedDate { get; set; }
}