namespace BudgetExpenseSystem.Model.Dto.Requests;

public class UpdateCategoryRequset
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string? Description { get; set; }
}