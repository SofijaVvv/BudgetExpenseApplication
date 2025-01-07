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
			new Role { Name = "Admin", CreatedDate = DateOnly.FromDateTime(DateTime.UtcNow) },
			new Role { Name = "User", CreatedDate = DateOnly.FromDateTime(DateTime.UtcNow) }
		};

		context.Roles.AddRange(roles);
		context.SaveChanges();
	}
}
