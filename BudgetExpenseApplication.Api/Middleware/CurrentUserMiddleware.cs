using BudgetExpenseApplication.Service.Interfaces;

namespace BudgetExpenseSystem.Api.Middleware;

public class CurrentUserMiddleware
{
	private readonly RequestDelegate _next;

	public CurrentUserMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task InvokeAsync(HttpContext context, ICurrentUserService currentUserService)
	{
		currentUserService.CurrentUser = context.User;
		await _next(context);
	}
}
