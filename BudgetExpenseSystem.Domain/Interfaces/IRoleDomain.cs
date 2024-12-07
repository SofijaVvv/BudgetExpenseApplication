using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseSystem.Domain.Interfaces;

public interface IRoleDomain
{
	Task<List<Role>> GetAllAsync();
	Task<Role> GetByIdAsync(int id);
	Task<Role> AddAsync(Role role);
	Task Update(int roleId, UpdateRoleRequest updateRoleRequest);
	Task DeleteAsync(int id);
}