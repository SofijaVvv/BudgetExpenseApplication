using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Dto.Response;
using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseSystem.Model.Extentions;

public static class CategoryExtentions
{
    public static Category ToCategory(this CategoryRequest request)
    {
        return new Category
        {
            Name = request.Name,
            Description = request.Description
        };
    }

    public static Category ToCategory(this UpdateCategoryRequset request)
    {
        return new Category
        {
            Id = request.Id,
            Name = request.Name,
            Description = request.Description
        };
    }

    public static CategoryResponse ToResponse(this Category response)
    {
        return new CategoryResponse
        {
            Id = response.Id,
            Name = response.Name,
            Description = response.Description
        };
    }
    
    public static List<CategoryResponse> ToResponse(this List<Category> response)
    {
        return response.Select(category => category.ToResponse()).ToList();
    }
}