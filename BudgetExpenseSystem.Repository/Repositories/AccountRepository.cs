using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;

namespace BudgetExpenseSystem.Repository.Repositories;

public class AccountRepository : GenericRepository<Account>, IAccountRepository
{
	public AccountRepository(ApplicationDbContext context) : base(context)
	{
	}
}
