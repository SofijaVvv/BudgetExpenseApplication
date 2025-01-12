using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Design;

namespace BudgetExpenseApplication.Repository;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
	private const string ErrorMessage = "Connection string is not configured properly or is missing.";

	public ApplicationDbContext CreateDbContext(string[]? args = null)
	{
		var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

		var configuration = new ConfigurationBuilder()
			.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../BudgetExpenseSystem.Api"))
			.AddJsonFile("appsettings.json", false)
			.Build();

		var connectionString = configuration.GetConnectionString("ConnectionDefault");
		var mySqlVersion = ServerVersion.Parse("10.4.28-mariadb");
		optionsBuilder.UseMySql(connectionString ?? throw new InvalidOperationException(ErrorMessage), mySqlVersion);

		return new ApplicationDbContext(optionsBuilder.Options);
	}
}
