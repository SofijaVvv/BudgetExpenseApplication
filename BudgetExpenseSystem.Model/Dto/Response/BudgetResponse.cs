namespace BudgetExpenseSystem.Model.Dto.Response;

public class BudgetResponse
{
	public int Id { get; set; }

	public CategoryResponse Category { get; set; }

	public BudgetTypeResponse BudgetType { get; set; }

	public decimal Amount { get; set; }

	public DateOnly CreatedDate { get; set; }
}