using System.ComponentModel.DataAnnotations;

namespace BudgetExpenseSystem.Model.Models;

public class Budget
{
	[Key] public int Id { get; set; }

	public int UserId { get; set; }

	public int CategoryId { get; set; }

	public int BudgetTypeId { get; set; }

	[Required] public decimal Amount { get; set; }

	public DateOnly CreatedDate { get; set; }

	public virtual User User { get; set; }
	public virtual Category Category { get; set; }
	public virtual BudgetType BudgetType { get; set; }
}