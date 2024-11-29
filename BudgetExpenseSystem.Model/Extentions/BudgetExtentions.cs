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
            BudgetTypeId = request.BudgetTypeId,
            Amount = request.Amount,
            CreatedDate = request.CreatedDate
        };
    }

    public static Budget ToBudget(this UpdateBudgetRequest request)
    {
        return new Budget
        {
            Id = request.Id,
            UserId = request.UserId,
            CategoryId = request.CategoryId,
            BudgetTypeId = request.BudgetTypeId,
            Amount = request.Amount,
            CreatedDate = request.CreatedDate
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
            BudgetType = new BudgetTypeResponse
            {
                Id = response.BudgetType.Id,
                Name = response.BudgetType.Name
            },
            Amount = response.Amount,
            CreatedDate = response.CreatedDate
        };
    }
    
    public static List<BudgetResponse> ToResponse(this List<Budget> response)
    {
        return response.Select(budget => budget.ToResponse()).ToList();
    }
}