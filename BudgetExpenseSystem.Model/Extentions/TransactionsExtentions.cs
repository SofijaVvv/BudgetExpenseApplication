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
			Currency = request.Currency,
			Amount = request.Amount,
		};
	}


	public static TransactionResponse ToResponse(this Transaction response)
	{
		return new TransactionResponse
		{
			Id = response.Id,
			Account = new AccountResponse
			{
				Id = response.Account.Id,
				Balance = Math.Round(response.Account.Balance, 2)
			},
			Category = new CategoryResponse
			{
				Id = response.Category.Id,
				Name = response.Category.Name,
				Description = response.Category.Description
			},
			Currency = response.Currency,
			BudgetId = response.BudgetId,
			Amount = Math.Round(response.Amount, 2),
			CreatedAt = response.CreatedAt
		};
	}

	public static List<TransactionResponse> ToResponse(this List<Transaction
	> response)
	{
		return response.Select(transaction => transaction.ToResponse()).ToList();
	}
}
