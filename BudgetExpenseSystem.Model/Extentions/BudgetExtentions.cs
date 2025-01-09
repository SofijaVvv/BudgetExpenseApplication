using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Dto.Response;
using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseSystem.Model.Extentions;

public static class BudgetExtentions
{
	public static Budget ToBudget(this BudgetRequest request)
	{
		return new Budget
		{
			UserId = request.UserId,
			CategoryId = request.CategoryId,
			Amount = request.Amount,
		};
	}

	public static BudgetResponse ToResponse(this Budget response)
	{
		return new BudgetResponse
		{
			Id = response.Id,
			UserId = response.UserId,
			Category = new CategoryResponse
			{
				Id = response.Category.Id,
				Name = response.Category.Name
			},
			Amount = response.Amount,
			CreatedAt = response.CreatedAt
		};
	}


	public static List<BudgetResponse> ToResponse(this List<Budget> response)
	{
		return response.Select(budget => budget.ToResponse()).ToList();
	}
}
