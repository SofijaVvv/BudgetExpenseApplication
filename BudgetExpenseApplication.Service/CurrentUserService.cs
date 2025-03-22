using System.Security.Claims;
using BudgetExpenseApplication.Service.Interfaces;

namespace BudgetExpenseApplication.Service;

public class CurrentUserService : ICurrentUserService
{
	private ClaimsPrincipal? CurrentUser { get; set; }


	public int? GetUserId()
	{
		var userIdClaim = CurrentUser?.FindFirst(ClaimTypes.NameIdentifier);
		var userId = int.Parse(userIdClaim?.Value);

		return userId;
	}

	public void Set(ClaimsPrincipal user)
	{
		CurrentUser = user;
	}
}
