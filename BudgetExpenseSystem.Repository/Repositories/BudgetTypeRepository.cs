using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;

namespace BudgetExpenseSystem.Repository.Repositories;

public class BudgetTypeRepository : GenericRepository<BudgetType>, IBudgetTypeRepository
{
	public BudgetTypeRepository(ApplicationDbContext context) : base(context)
	{
	}
}
