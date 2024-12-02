using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseSystem.Domain.Interfaces;

public interface IAccountDomain
{
	Task<List<Account>> GetAllAsync();
	Task<Account> GetByIdAsync(int id);
	Task<Account> AddAsync(Account account);
	Task Update(int accountId, UpdateAccountRequest updateAccountRequest);
	Task DeleteAsync(int id);
}
