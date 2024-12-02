using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseSystem.Repository.Interfaces;

public interface IBudgetRepository : IGenericRepository<Budget>
{
	Task<List<Budget>> GetAllBudgetsAsync();
	Task<Budget?> GetBudgetByIdAsync(int id);
}
