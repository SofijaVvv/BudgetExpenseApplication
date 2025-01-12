using System.Security.Claims;
using BudgetExpenseSystem.Model.Constants;
using Microsoft.AspNetCore.Authorization;

namespace BudgetExpenseSystem.Api.Extentions;

public static class AuthorizationPolicyExtensions
{
	public static void AddPolicies(this AuthorizationOptions options)
	{
		options.AddPolicy("AdminOnly", policy =>
			policy.RequireClaim(ClaimTypes.Role, RoleConstants.AdminName));

		options.AddPolicy("UserOnly", policy =>
			policy.RequireClaim(ClaimTypes.Role, RoleConstants.UserName));
	}
}
