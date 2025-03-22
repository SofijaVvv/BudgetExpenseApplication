using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseApplication.Repository.Interfaces;

public interface IAccountRepository : IGenericRepository<Account>
{
	Task<Account?> GetByUserIdAsync(int? userId);
}
