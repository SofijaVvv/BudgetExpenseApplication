using System.ComponentModel.DataAnnotations;

namespace BudgetExpenseSystem.Model.Models;

public class Budget
{
	[Key]
	public int Id { get; set; }

	[Required]
	public string Name { get; set; }

	public int UserId { get; set; }

	[Required]
	public int CategoryId { get; set; }

	[Required]
	public decimal Amount { get; set; }

	public DateTime CreatedAt { get; set; }

	public virtual Category Category { get; set; }

	public virtual User User { get; set; }
}
