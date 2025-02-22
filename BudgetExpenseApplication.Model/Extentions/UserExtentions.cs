using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Dto.Response;
using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseSystem.Model.Extentions;

public static class UserExtentions
{
	public static User ToUser(this UserRequest request)
	{
		return new User
		{
			Email = request.Email,
		};
	}

	public static UserResponse ToResponse(this User response)
	{
		return new UserResponse
		{
			Id = response.Id,
			Email = response.Email,
			Token = new TokenResponse(),
			Role = new RoleResponse
			{
				Id = response.Role.Id,
				Name = response.Role.Name,
				CreatedDate = response.Role.CreatedAt
			}
		};
	}

	public static List<UserResponse> ToResponse(this List<User> response)
	{
		return response.Select(user => user.ToResponse()).ToList();
	}
}
