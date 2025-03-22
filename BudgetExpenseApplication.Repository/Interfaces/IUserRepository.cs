using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseApplication.Repository.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
	Task<User?> GetUserEmailAsync(string email);
}
