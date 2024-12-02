using System.ComponentModel.DataAnnotations;

namespace BudgetExpenseSystem.Model.Models;

public class Account
{
	[Key] public int Id { get; set; }

	//TODO Add UserId column
	[Required] public decimal Balance { get; set; }

	public DateOnly CreatedDate { get; set; }
}