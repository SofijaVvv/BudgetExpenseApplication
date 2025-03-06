using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseApplication.Repository.Interfaces;

public interface ITransactionRepository : IGenericRepository<Transaction>
{
	Task<List<Transaction>> GetAllTransactionsAsync();
	Task<Transaction?> GetTransactionById(int? id);
}
