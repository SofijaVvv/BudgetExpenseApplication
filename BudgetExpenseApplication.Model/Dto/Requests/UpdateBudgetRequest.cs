namespace BudgetExpenseSystem.Model.Dto.Requests;

public class UpdateBudgetRequest
{
	public string Name { get; set; }
	public int CategoryId { get; set; }


	public decimal Amount { get; set; }
}
