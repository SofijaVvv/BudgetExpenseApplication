using System.ComponentModel.DataAnnotations;

namespace BudgetExpenseSystem.Model.Models;

public class ScheduledTransaction
{
	[Key] public int Id { get; set; }

	public int AccountId { get; set; }
	public int CategoryId { get; set; }
	public int BudgetId { get; set; }

	[Required] public decimal Amount { get; set; }
	[Required] public DateTime ScheduledDate { get; set; }
	public bool IsRecurring { get; set; }

	public virtual Account Account { get; set; }
	public virtual Category Category { get; set; }
	public virtual Budget Budget { get; set; }
}
