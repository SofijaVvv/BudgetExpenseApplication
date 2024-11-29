using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Dto.Response;
using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseSystem.Model.Extentions;

public static class RoleExtentions
{
    public static Role ToRole(this RoleRequest request)
    {
        return new Role
        {
           Name = request.Name,
           CreatedDate = request.CreatedDate
        };
    }

    public static Role ToRole(this UpdateRoleRequest request)
    {
        return new Role
        { 
            Id = request.Id,
            Name = request.Name,
            CreatedDate = request.CreatedDate
        };
    }

    public static RoleResponse ToResponse(this Role response)
    {
        return new RoleResponse
        {
            Id = response.Id,
            Name = response.Name,
            CreatedDate = response.CreatedDate
        };
    }
    
    public static List<RoleResponse> ToResponse(this List<Role> response)
    {
        return response.Select(notification => notification.ToResponse()).ToList();
    }
}