using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseSystem.Domain.Interfaces;

public interface IScheduledTransactionHandlerDomain
{
	Task ScheduleTransactionAsync(ScheduledTransaction transaction);
	Task DeleteScheduledTransactionAsync(int scheduledTransactionId);

	Task UpdateScheduledTransactionAsync(int scheduledTransactionId,
		UpdateScheduleTransactionRequest updateScheduleTransaction);
}
