using Microsoft.OpenApi.Models;

namespace BudgetExpenseSystem.Api.Extentions;

public static class SwaggerExtentions
{
	public static IServiceCollection AddSwaggerWithJwtAuth(this IServiceCollection services)
	{
		services.AddSwaggerGen(c =>
		{
			c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

			c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
			{
				In = ParameterLocation.Header,
				Description = "Enter 'Bearer' [space] and your token",
				Name = "Authorization",
				Type = SecuritySchemeType.ApiKey,
				Scheme = "Bearer"
			});

			c.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = "Bearer"
						},
						Scheme = "Bearer",
						Name = "Authorization",
						In = ParameterLocation.Header
					},
					new string[] { }
				}
			});
		});

		return services;
	}
}