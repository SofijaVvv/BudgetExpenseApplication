using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace BudgetExpenseSystem.Api.Extentions;

public static class JwtAuthExtention
{
	public static void AddJwtAuthentication(this WebApplicationBuilder builder)
	{
		var secretKey = builder.Configuration["JwtSettings:SecretKey"]
		                ?? throw new Exception("JwtSettings:SecretKey not found in configuration");
		var key = Encoding.UTF8.GetBytes(secretKey);

		builder.Services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(x =>
			{
				x.RequireHttpsMetadata = true;
				x.SaveToken = true;
				x.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false,
					ValidateLifetime = true,
					ClockSkew = TimeSpan.Zero
				};
			});
	}
}
