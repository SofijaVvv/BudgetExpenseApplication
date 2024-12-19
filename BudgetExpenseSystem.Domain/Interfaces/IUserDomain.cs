using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Dto.Response;
using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseSystem.Domain.Interfaces;

public interface IUserDomain
{
	Task<List<User>> GetAllAsync();
	Task<User> GetByIdAsync(int id);

	Task<UserResponse?> LoginUserAsync(string email, string password);

	Task<User> RegisterUserAsync(UserRequest userRequest);
	Task DeleteAsync(int id);
}
