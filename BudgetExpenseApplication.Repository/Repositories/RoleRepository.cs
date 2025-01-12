using BudgetExpenseApplication.Repository.Interfaces;
using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseApplication.Repository.Repositories;

public class RoleRepository : GenericRepository<Role>, IRoleRepository
{
	public RoleRepository(ApplicationDbContext context) : base(context)
	{
	}
}
