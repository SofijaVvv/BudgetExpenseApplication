namespace BudgetExpenseSystem.Model.Dto.Response;

public class BudgetResponse
{
	public int Id { get; set; }

	public int UserId { get; set; }
	public CategoryResponse Category { get; set; }


	public decimal Amount { get; set; }

	public DateTime CreatedAt { get; set; }
}
