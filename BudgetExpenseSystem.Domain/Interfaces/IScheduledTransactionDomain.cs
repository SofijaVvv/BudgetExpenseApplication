using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseSystem.Domain.Interfaces;

public interface IScheduledTransactionDomain
{
	Task<ScheduledTransaction> AddAsync(ScheduledTransaction scheduledTransaction);
	Task<List<ScheduledTransaction>> GetAllAsync();
	Task<ScheduledTransaction> GetByIdAsync(int id);
	Task Update(int scheduledTransactionId, UpdateScheduleTransactionRequest updateScheduleTransactionRequest);
	Task DeleteAsync(int id);
}
