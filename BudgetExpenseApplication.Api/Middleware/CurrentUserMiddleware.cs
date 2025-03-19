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
		if (context.User?.Identity?.IsAuthenticated == true)
		{
			var claims = string.Join(", ", context.User.Claims.Select(c => $"{c.Type}:{c.Value}"));
			Console.WriteLine($"User is authenticated. Claims: {claims}");
		}
		else
		{
			Console.WriteLine("User is NOT authenticated.");
		}
		currentUserService.CurrentUser = context.User;
		await _next(context);
	}
}
