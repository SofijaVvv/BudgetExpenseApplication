using System.Security.Claims;
using BudgetExpenseApplication.Service.Interfaces;

namespace BudgetExpenseApplication.Service;

public class CurrentUserService : ICurrentUserService
{
	public ClaimsPrincipal? CurrentUser { get; set; }
}
