using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Dto.Response;
using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseSystem.Model.Extentions;

public static class UserExtentions
{
	public static User ToUser(this RegisterRequest request)
	{
		return new User
		{
			Email = request.Email,
			FullName = request.FullName
		};
	}

	public static UserResponse ToResponse(this User response)
	{
		return new UserResponse
		{
			Id = response.Id,
			FullName = response.FullName,
			Email = response.Email,
			Role = response.Role.Name
		};
	}

	public static List<UserResponse> ToResponse(this List<User> response)
	{
		return response.Select(user => user.ToResponse()).ToList();
	}
}
