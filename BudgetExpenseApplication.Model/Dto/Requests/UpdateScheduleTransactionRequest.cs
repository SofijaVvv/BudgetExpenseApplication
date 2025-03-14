namespace BudgetExpenseSystem.Model.Dto.Requests;

public class UpdateScheduleTransactionRequest
{
	public int CategoryId { get; set; }
	public int BudgetId { get; set; }
	public decimal Amount { get; set; }
	public DateTime ScheduledDate { get; set; }
	public bool IsRecurring { get; set; }
}
