namespace BudgetExpenseSystem.Model.Dto.Requests;

public class BudgetRequest
{
	public int CategoryId { get; set; }

	public int UserId { get; set; }


	public decimal Amount { get; set; }

}
