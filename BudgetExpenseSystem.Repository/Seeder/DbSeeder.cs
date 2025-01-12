using System.Security.Cryptography;
using System.Text;
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

		if (!context.Users.Any())
		{
			SeedUsers(context);
		}

		if (!context.Categories.Any())
		{
			SeedCategories(context);
		}
	}

	private static void SeedRoles(ApplicationDbContext context)
	{
		var roles = new List<Role>
		{
			new() {Id = RoleConstants.AdminId, Name = RoleConstants.AdminName, CreatedAt = DateTime.UtcNow },
			new() {Id= RoleConstants.UserId,  Name = RoleConstants.UserName, CreatedAt = DateTime.UtcNow }
		};

		context.Roles.AddRange(roles);
		context.SaveChanges();
	}

	private static void SeedUsers(ApplicationDbContext context)
	{
		string salt = "your-predefined-base64-salt-here";
		var users = new List<User>
		{
			new() { RoleId = RoleConstants.AdminId, Email = "admin@gmail.com", PasswordSalt = salt,
				PasswordHash = HashPassword("admin123", salt)},
			new() { RoleId = RoleConstants.UserId, Email = "user@gmail.com", PasswordSalt = salt, PasswordHash = HashPassword("user123", salt)}
		};

		context.Users.AddRange(users);
		context.SaveChanges();
	}

	private static void SeedCategories(ApplicationDbContext context)
	{
		var categories = new List<Category>
		{
			new() { Name = "Food", Description = "Grocery shopping for food"},
			new() { Name = "Electricity", Description = "Monthly electricity bill"},
			new() { Name = "Rent", Description = "Monthly rent payment"},
			new() { Name = "Water", Description = "Monthly water bill"},
			new() { Name = "Gas", Description = "Gas supply expenses"},
			new() { Name = "Internet", Description = "Monthly internet subscription"},
			new() { Name = "Phone", Description = "Mobile and landline bills"},
			new() { Name = "Transportation", Description = "Public transport, fuel, and car maintenance"},
			new() { Name = "Entertainment", Description = "Movies, concerts, and other leisure activities"},
			new() { Name = "Dining Out", Description = "Restaurants, cafes, and takeout"},
			new() { Name = "Other", Description = "Other expenses not categorized"},
		};

		context.Categories.AddRange(categories);
		context.SaveChanges();
	}

	private static string HashPassword(string password, string salt)
	{
		using (var hmac = new HMACSHA512(Convert.FromBase64String(salt)))
		{
			var passwordBytes = Encoding.UTF8.GetBytes(password);
			var hash = hmac.ComputeHash(passwordBytes);
			return Convert.ToBase64String(hash);
		}
	}

}
