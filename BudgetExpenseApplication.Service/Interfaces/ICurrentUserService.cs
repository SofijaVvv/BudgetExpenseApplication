using System.Security.Claims;

namespace BudgetExpenseApplication.Service.Interfaces;

public interface ICurrentUserService
{
	ClaimsPrincipal? CurrentUser { get; set; }
}
