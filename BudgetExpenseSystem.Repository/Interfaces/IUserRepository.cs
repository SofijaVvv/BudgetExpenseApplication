using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseSystem.Repository.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
	Task<List<User>> GetAllUsersAsync();
	Task<User?> GetUserByIdAsync(int id);
}
