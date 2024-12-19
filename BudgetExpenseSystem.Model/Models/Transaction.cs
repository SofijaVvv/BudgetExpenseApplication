using System.ComponentModel.DataAnnotations;

namespace BudgetExpenseSystem.Model.Models;

public class Transaction
{
	[Key] public int Id { get; set; }

	[Required] public int AccountId { get; set; }

	[Required] public int CategoryId { get; set; }

	[Required] public int BudgetId { get; set; }

	[Required] public decimal Amount { get; set; }

	public DateOnly TransactionDate { get; set; }

	public virtual Account Account { get; set; }
	public virtual Category Category { get; set; }
	public virtual Budget Budget { get; set; }
}