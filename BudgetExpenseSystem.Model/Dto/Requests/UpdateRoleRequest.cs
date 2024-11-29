namespace BudgetExpenseSystem.Model.Dto.Requests;

public class UpdateRoleRequest
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public DateOnly CreatedDate { get; set; } 
}