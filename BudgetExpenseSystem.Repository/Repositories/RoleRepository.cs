using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;

namespace BudgetExpenseSystem.Repository.Repositories;

public class RoleRepository : GenericRepository<Role>, IRoleRepository
{
	public RoleRepository(ApplicationDbContext context) : base(context)
	{
	}
}
