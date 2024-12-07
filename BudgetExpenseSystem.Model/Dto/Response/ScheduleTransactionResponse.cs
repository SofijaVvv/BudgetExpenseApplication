namespace BudgetExpenseSystem.Model.Dto.Response;

public class ScheduleTransactionResponse
{
	public int Id { get; set; }
	public AccountResponse Account { get; set; }
	public CategoryResponse Category { get; set; }
	public int BudgetId { get; set; }
	public decimal Amount { get; set; }
	public DateTime ScheduledDate { get; set; }
	public bool IsRecurring { get; set; }
}
