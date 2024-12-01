using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Dto.Response;
using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseSystem.Model.Extentions;

public static class TransactionsExtentions
{
	public static Transaction ToTransaction(this TransactionRequest request)
	{
		return new Transaction
		{
			AccountId = request.AccountId,
			CategoryId = request.CategoryId,
			BudgetId = request.BudgetId,
			Amount = request.Amount,
			TransactionDate = request.TransactionDate
		};
	}


	public static TransactionResponse ToResponse(this Transaction response)
	{
		return new TransactionResponse
		{
			Id = response.Id,
			AccountId = response.AccountId,
			Category = new CategoryResponse
			{
				Id = response.Category.Id,
				Name = response.Category.Name
			},
			BudgetId = response.BudgetId,
			Amount = response.Amount,
			TransactionDate = response.TransactionDate
		};
	}

	public static List<TransactionResponse> ToResponse(this List<Transaction
	> response)
	{
		return response.Select(notification => notification.ToResponse()).ToList();
	}
}
