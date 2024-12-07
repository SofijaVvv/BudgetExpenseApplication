using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Dto.Response;
using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseSystem.Domain.Interfaces;

public interface IBudgetDomain
{
	Task<List<Budget>> GetAllAsync();
	Task<Budget> GetByIdAsync(int id);
	Task<Budget> AddAsync(Budget budget);
	Task Update(int budgetId, UpdateBudgetRequest updateBudgetRequest);
	Task DeleteAsync(int id);
}