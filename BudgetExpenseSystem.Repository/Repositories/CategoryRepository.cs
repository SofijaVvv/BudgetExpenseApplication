using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;

namespace BudgetExpenseSystem.Repository.Repositories;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
	public CategoryRepository(ApplicationDbContext context) : base(context)
	{
	}
}
