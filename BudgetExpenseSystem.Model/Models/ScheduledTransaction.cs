using System.ComponentModel.DataAnnotations;

namespace BudgetExpenseSystem.Model.Models;

public class ScheduledTransaction
{
	[Key]
	public int Id { get; set; }

	[Required]
	public int AccountId { get; set; }
	[Required]
	public int CategoryId { get; set; }
	[Required]
	public int BudgetId { get; set; }

	[Required]
	[MaxLength(3)]
	public string Currency { get; set; }

	public string? JobId { get; set; }

	[Required]
	public decimal Amount { get; set; }
	[Required]
	public DateTime ScheduledDate { get; set; }
	[Required]
	public bool IsRecurring { get; set; }

	public virtual Account Account { get; set; }
	public virtual Category Category { get; set; }
	public virtual Budget Budget { get; set; }
}
