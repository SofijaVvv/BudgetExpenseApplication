namespace BudgetExpenseSystem.Model.Dto.Requests;

public class ScheduleTransactionRequest
{
	public int AccountId { get; set; }
	public int CategoryId { get; set; }
	public int BudgetId { get; set; }
	public decimal Amount { get; set; } // TODO decimal
	public DateTime ScheduledDate { get; set; } // TODO merge into DateTime
	public bool IsRecurring { get; set; }
}
