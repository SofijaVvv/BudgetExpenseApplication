using System.ComponentModel.DataAnnotations;

namespace BudgetExpenseSystem.Model.Models;

public class Category
{
	[Key] public int Id { get; set; }

	[Required] public string Name { get; set; }

	public string? Description { get; set; }
}