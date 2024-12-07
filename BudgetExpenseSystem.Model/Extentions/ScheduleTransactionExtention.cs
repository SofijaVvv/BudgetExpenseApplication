using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Dto.Response;
using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseSystem.Model.Extentions;

public static class ScheduleTransactionExtention
{
	public static ScheduledTransaction ToScheduledTransaction(this ScheduleTransactionRequest request)
	{
		return new ScheduledTransaction
		{
			AccountId = request.AccountId,
			CategoryId = request.CategoryId,
			BudgetId = request.BudgetId,
			Amount = request.Amount,
			ScheduledDate = request.ScheduledDate,
			IsRecurring = request.IsRecurring
		};
	}


	public static ScheduleTransactionResponse ToResponse(this ScheduledTransaction response)
	{
		return new ScheduleTransactionResponse
		{
			Id = response.Id,
			Account = new AccountResponse
			{
				Id = response.Account.Id,
				Balance = response.Account.Balance
			},
			Category = new CategoryResponse
			{
				Id = response.Category.Id,
				Name = response.Category.Name,
				Description = response.Category.Description
			},
			BudgetId = response.BudgetId,
			Amount = response.Amount,
			ScheduledDate = response.ScheduledDate,
			IsRecurring = response.IsRecurring
		};
	}

	public static List<ScheduleTransactionResponse> ToResponse(this List<ScheduledTransaction> response)
	{
		return response.Select(scheduledTransaction => scheduledTransaction.ToResponse()).ToList();
	}
}
