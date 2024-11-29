namespace BudgetExpenseSystem.Model.Dto.Requests;

public class UpdateTransactionRequest
{
    public int Id { get; set; }
    
    public int AccountId { get; set; }
    
    public int CategoryId { get; set; }
    
    public int BudgetId { get; set; }
    
    public decimal Amount { get; set; }
    
    public DateOnly TransactionDate { get; set; }
}