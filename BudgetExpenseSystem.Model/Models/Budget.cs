using System.ComponentModel.DataAnnotations;

namespace BudgetExpenseSystem.Model.Models;

public class Budget
{
	[Key] public int Id { get; set; }

	//TODO Add UserId column

	[Required] public int CategoryId { get; set; }

	[Required] public int BudgetTypeId { get; set; }

	[Required] public decimal Amount { get; set; }

	public DateOnly CreatedDate { get; set; }

	public virtual Category Category { get; set; }
	public virtual BudgetType BudgetType { get; set; }
}