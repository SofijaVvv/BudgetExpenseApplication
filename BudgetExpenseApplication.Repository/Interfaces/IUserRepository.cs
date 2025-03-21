using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseApplication.Repository.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
	Task<List<User>> GetAllUsersAsync();
	Task<User?> GetUserEmailAsync(string email);
	Task<User?> GetUserByIdAsync(int? id);
}
