using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseApplication.Repository.Interfaces;

public interface IAccountRepository : IGenericRepository<Account>
{
	Task<List<Account>> GetAllAccountsAsync();
	Task<Account?> GetAccountByIdAsync(int? id);
	Task<Account?> GetByUserIdAsync(int? userId);
}
