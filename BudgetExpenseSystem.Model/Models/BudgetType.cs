using System.ComponentModel.DataAnnotations;

namespace BudgetExpenseSystem.Model.Models;

public class BudgetType
{
	[Key] public int Id { get; set; }

	[Required] public string Name { get; set; }
}