using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseSystem.Domain.Interfaces;

public interface ITransactionDomain
{
	Task<List<Transaction>> GetAllAsync();
	Task<Transaction> GetByIdAsync(int id);
	Task<Transaction> AddAsync(TransactionRequest transactionRequest);
	Task Update(int transactionId, UpdateTransactionRequest updateTransactionRequest);
	Task DeleteAsync(int id);
}
