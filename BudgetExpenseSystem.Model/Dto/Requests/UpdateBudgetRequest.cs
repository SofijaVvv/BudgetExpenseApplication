namespace BudgetExpenseSystem.Model.Dto.Requests;

public class UpdateBudgetRequest
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    public int CategoryId { get; set; }
    
    public int BudgetTypeId { get; set; }
    
    public decimal Amount { get; set; }
    
    public DateOnly CreatedDate { get; set; }
}