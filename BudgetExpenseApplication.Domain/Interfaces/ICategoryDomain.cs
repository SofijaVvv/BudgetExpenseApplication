using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseSystem.Domain.Interfaces;

public interface ICategoryDomain
{
	Task<List<Category>> GetAllAsync();
	Task<Category> GetByIdAsync(int id);
	Task<Category> AddAsync(Category category);
	Task Update(int categoryId, UpdateCategoryRequset updateCategoryRequset);
	Task DeleteAsync(int id);
}
