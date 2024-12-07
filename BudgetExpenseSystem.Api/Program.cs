using BudgetExpenseSystem.Api.Filters;
using BudgetExpenseSystem.Background;
using BudgetExpenseSystem.Background.Interface;
using BudgetExpenseSystem.Domain.Domains;
using BudgetExpenseSystem.Domain.Interfaces;
using BudgetExpenseSystem.Repository;
using BudgetExpenseSystem.Repository.Interfaces;
using BudgetExpenseSystem.Repository.Repositories;
using Hangfire;
using Hangfire.MySql;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers(options =>
	options.Filters.Add<GlobalExceptionFilter>()
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//builder.Services.AddDomains();
//builder.Services.AddRepositories();
builder.Services.AddScoped<IRoleDomain, RoleDomain>();
builder.Services.AddScoped<IAccountDomain, AccountDomain>();
builder.Services.AddScoped<IUserDomain, UserDomain>();
builder.Services.AddScoped<IBudgetDomain, BudgetDomain>();
builder.Services.AddScoped<IBudgetTypeDomain, BudgetTypeDomain>();
builder.Services.AddScoped<ICategoryDomain, CategoryDomain>();
builder.Services.AddScoped<INotificationDomain, NotificationDomain>();
builder.Services.AddScoped<ITransactionDomain, TransactionDomain>();
builder.Services.AddScoped<IBudgetRepository, BudgetRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IBudgetTypeRepository, BudgetTypeRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IScheduledTransactionDomain, ScheduledTransactionDomain>();
builder.Services.AddScoped<IScheduledTransactionRepository, ScheduledTransactionRepository>();
builder.Services.AddScoped<IScheduledTransactionService, ScheduledTransactionService>();
builder.Services.AddLogging();


var connectionString = builder.Configuration.GetConnectionString("ConnectionDefault")
                       ?? throw new Exception("Connection string 'ConnectionDefault' is not configured or is missing.");
var mySqlVersion = ServerVersion.Parse("10.4.28-mariadb");
builder.Services.AddDbContext<ApplicationDbContext>(options => { options.UseMySql(connectionString, mySqlVersion); });

builder.Services.AddHangfire(config =>
{
	config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
		.UseSimpleAssemblyNameTypeSerializer()
		.UseDefaultTypeSerializer()
		.UseStorage(new MySqlStorage(connectionString, new MySqlStorageOptions
		{
			TransactionIsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
			QueuePollInterval = TimeSpan.FromSeconds(15),
			JobExpirationCheckInterval = TimeSpan.FromHours(1),
			CountersAggregateInterval = TimeSpan.FromMinutes(5),
			PrepareSchemaIfNecessary = true,
			DashboardJobListLimit = 5000,
			TransactionTimeout = TimeSpan.FromMinutes(1)
		}));
});
builder.Services.AddHangfireServer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHangfireDashboard("/hangfire");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
