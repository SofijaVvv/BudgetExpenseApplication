namespace BudgetExpenseSystem.Model.Dto.Response;

public class TransactionResponse
{
    public int Id { get; set; }
    
    public int AccountId { get; set; }
    
    public CategoryResponse Category { get; set; }
    
    public int BudgetId { get; set; }
    
    public decimal Amount { get; set; }
    
    public DateOnly TransactionDate { get; set; }
}