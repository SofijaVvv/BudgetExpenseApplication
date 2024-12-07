using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseSystem.Repository.Interfaces;

public interface IScheduledTransactionRepository : IGenericRepository<ScheduledTransaction>
{
	Task<List<ScheduledTransaction>> GetAllScheduledTransactionsAsync();
	Task<ScheduledTransaction?> GetScheduledTransactionByIdAsync(int id);
}
