using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace BudgetExpenseSystem.Api.Extentions;

public static class AuthorizationPolicyExtensions
{
	public static void AddPolicies(this AuthorizationOptions options)
	{
		options.AddPolicy("AdminOnly", policy =>
			policy.RequireClaim(ClaimTypes.Role, "Admin"));

		options.AddPolicy("UserOnly", policy =>
			policy.RequireClaim(ClaimTypes.Role, "User"));
	}
}