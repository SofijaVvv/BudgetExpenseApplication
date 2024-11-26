using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Design;
using BudgetExpenseSystem.Repository;

namespace BudgetExpenseSystem.Repository;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[]? args = null)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../BudgetExpenseSystem.Api"))
            .AddJsonFile("appsettings.json", false)
            .Build();

        var connectionString = configuration.GetConnectionString("ConnectionDefault");
        var mySqlVersion = ServerVersion.Parse("10.4.28-mariadb");
        optionsBuilder.UseMySql(connectionString!, mySqlVersion);

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}