using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Dto.Response;
using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseSystem.Domain.Interfaces;

public interface IBudgetTypeDomain
{
	Task<List<BudgetType>> GetAllAsync();
	Task<BudgetType> GetByIdAsync(int id);
	Task<BudgetType> AddAsync(BudgetType budgetType);
	Task Update(int budgetTypeId, UpdateBudgetTypeRequest updateBudgetTypeRequest);
	Task DeleteAsync(int id);
}