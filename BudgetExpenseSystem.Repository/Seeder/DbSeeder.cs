using BudgetExpenseSystem.Model.Constants;
using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseSystem.Repository.Seeder;
public static class DbSeeder
{
	public static void Seed(ApplicationDbContext context)
	{
		context.Database.EnsureCreated();

		if (!context.Roles.Any())
		{
			SeedRoles(context);
		}
	}

	private static void SeedRoles(ApplicationDbContext context)
	{
		var roles = new List<Role>
		{
			new Role {Id = RoleConstants.AdminId, Name = RoleConstants.AdminName, CreatedDate = DateOnly.FromDateTime(DateTime.UtcNow) },
			new Role {Id= RoleConstants.UserId,  Name = RoleConstants.UserName, CreatedDate = DateOnly.FromDateTime(DateTime.UtcNow) }
		};

		context.Roles.AddRange(roles);
		context.SaveChanges();
	}
}
