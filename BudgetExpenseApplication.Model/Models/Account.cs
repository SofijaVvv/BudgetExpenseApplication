using System.ComponentModel.DataAnnotations;

namespace BudgetExpenseSystem.Model.Models;

public class Account
{
	[Key]
	public int Id { get; set; }

	public int UserId { get; set; }

	[Required]
	public decimal Balance { get; set; }

	[Required]
	[MaxLength(3)]
	public string Currency { get; set; } = "EUR";

	public DateTime CreatedAt { get; set; }

	public virtual User User { get; set; }
}
