using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseSystem.Repository.Interfaces;

public interface IAccountRepository : IGenericRepository<Account>
{
	Task<List<Account>> GetAllAccountsAsync();
	Task<Account?> GetAccountByIdAsync(int id);
}
