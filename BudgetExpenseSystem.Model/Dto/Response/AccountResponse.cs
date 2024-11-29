using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseSystem.Model.Dto.Response;

public class AccountResponse
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    public decimal Balancee { get; set; }
    
    public DateOnly CreatedDate { get; set; }
}