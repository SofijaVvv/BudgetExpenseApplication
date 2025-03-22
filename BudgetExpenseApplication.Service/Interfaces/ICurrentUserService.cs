
using System.Security.Claims;

namespace BudgetExpenseApplication.Service.Interfaces;

public interface ICurrentUserService
{
      int? GetUserId();
      void Set(ClaimsPrincipal user);
}
