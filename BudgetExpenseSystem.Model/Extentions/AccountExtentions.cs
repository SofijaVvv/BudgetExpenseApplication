using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Dto.Response;
using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseSystem.Model.Extentions;

public static class AccountExtentions
{
	public static Account ToAccount(this AccountRequest request)
	{
		return new Account
		{
			UserId = request.UserId,
			Balance = request.Balance,
			CreatedDate = request.CreatedDate
		};
	}

	public static AccountResponse ToResponse(this Account response)
	{
		return new AccountResponse
		{
			Id = response.Id,
			UserId = response.UserId,
			Balance = response.Balance,
			CreatedDate = response.CreatedDate
		};
	}

	public static List<AccountResponse> ToResponse(this List<Account> response)
	{
		return response.Select(account => account.ToResponse()).ToList();
	}
}