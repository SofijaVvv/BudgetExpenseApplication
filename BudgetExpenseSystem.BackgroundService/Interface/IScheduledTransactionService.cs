using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseSystem.Background.Interface;

public interface IScheduledTransactionService
{
	Task ScheduleTransactionAsync(ScheduledTransaction transaction);
	Task DeleteScheduledTransactionAsync(int scheduledTransactionId);

	Task UpdateScheduledTransactionAsync(int scheduledTransactionId,
		UpdateScheduleTransactionRequest updateScheduleTransaction);
}
