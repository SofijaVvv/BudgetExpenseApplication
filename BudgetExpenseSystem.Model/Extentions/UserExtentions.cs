using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Dto.Response;
using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseSystem.Model.Extentions;

public static class UserExtentions
{
    public static User ToTransaction(this UserRequest request)
    {
        return new User
        {
           Email = request.Email,
           RoleId = request.RoleId
        };
    }

    public static UserResponse ToResponse(this User response)
    {
        return new UserResponse
        {
            Id = response.Id,
            Email = response.Email,
            RoleId = response.RoleId
        };
    }
    
    public static List<UserResponse> ToResponse(this List<User> response)
    {
        return response.Select(user => user.ToResponse()).ToList();
    }
}