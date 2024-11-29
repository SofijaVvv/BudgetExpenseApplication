using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Dto.Response;
using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseSystem.Model.Extentions;

public static class BudgetTypeExtentions
{
    public static BudgetType ToBudgetType(this BudgetTypeRequest request)
    {
        return new BudgetType
        {
            Name = request.Name
        };
    }

    public static BudgetType ToBudgetType(this UpdateBudgetTypeRequest request)
    {
        return new BudgetType
        {
            Id = request.Id,
            Name = request.Name
        };
    }

    public static BudgetTypeResponse ToResponse(this BudgetType response)
    {
        return new BudgetTypeResponse
        {
            Id = response.Id,
            Name = response.Name
        };
    }
    
    public static List<BudgetTypeResponse> ToResponse(this List<BudgetType> response)
    {
        return response.Select(budgetType => budgetType.ToResponse()).ToList();
    }
}