using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseSystem.Domain.Interfaces;

public interface IUserDomain
{
	Task<List<User>> GetAllAsync();
	Task<User> GetByIdAsync(int id);
	Task<User> AddAsync(User user);
	Task DeleteAsync(int id);
}
