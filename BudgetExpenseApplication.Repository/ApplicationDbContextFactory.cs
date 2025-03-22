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
			.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../BudgetExpenseApplication.Api"))
			.AddJsonFile("appsettings.json", false)
			.Build();

		var connectionString = configuration.GetConnectionString("Database");
		var mySqlVersion = ServerVersion.AutoDetect(connectionString);
		optionsBuilder.UseMySql(connectionString ?? throw new InvalidOperationException(ErrorMessage), mySqlVersion);

		return new ApplicationDbContext(optionsBuilder.Options);
	}
}
