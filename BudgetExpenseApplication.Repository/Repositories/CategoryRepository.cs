using BudgetExpenseApplication.Repository.Interfaces;
using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseApplication.Repository.Repositories;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
	public CategoryRepository(ApplicationDbContext context) : base(context)
	{
	}
}
