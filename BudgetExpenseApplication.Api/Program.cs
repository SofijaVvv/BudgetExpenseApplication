using BudgetExpenseSystem.Api.Extentions;
using BudgetExpenseSystem.Api.Filters;
using BudgetExpenseApplication.Repository;
using BudgetExpenseApplication.Repository.Seeder;
using BudgetExpenseApplication.Service;
using BudgetExpenseApplication.Service.Interfaces;
using BudgetExpenseApplication.Service.Mock;
using BudgetExpenseApplication.WebSocket.Hub;
using BudgetExpenseSystem.Api.Middleware;
using Hangfire;
using Hangfire.MySql;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("ConnectionDefault")
                       ?? throw new Exception("Connection string 'ConnectionDefault' is not configured or is missing.");
var mySqlVersion = ServerVersion.AutoDetect(connectionString);
builder.Services.AddDbContext<ApplicationDbContext>(options => { options.UseMySql(connectionString, mySqlVersion); });

builder.Services.AddControllers(options =>
	options.Filters.Add<GlobalExceptionFilter>()
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDomains();
builder.Services.AddRepositories();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddLogging();
builder.Services.AddHttpClient();

if (builder.Environment.IsDevelopment())
	builder.Services.AddScoped<ICurrencyConversionService, MockCurrencyConversionService>();
else
	builder.Services
		.AddScoped<ICurrencyConversionService, CurrencyConversionService>();

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
builder.Services.AddSwaggerWithJwtAuth();
builder.Services.AddHttpContextAccessor();

var secretKey = builder.Configuration["JwtSettings:SecretKey"]
                ?? throw new Exception("JwtSettings:SecretKey not found in configuration");

builder.Services.AddJwtAuthentication(secretKey);

builder.Services.AddAuthorization(
	options => options.AddPolicies()
);

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowSpecificOrigin", corsPolicyBuilder =>
	{
		corsPolicyBuilder.WithOrigins("http://localhost:4200")
			.AllowAnyMethod()
			.AllowAnyHeader()
			.AllowCredentials();
	});
});

builder.Services.AddSignalR();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
	DbSeeder.Seed(context);
}

app.UseCors("AllowSpecificOrigin");

app.MapHub<NotificationHub>("/notificationHub");
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	app.UseHangfireDashboard();
}

using (var scope = app.Services.CreateScope())
{
	var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
	DatabaseBackup databaseBackup = new DatabaseBackup(context);
	RecurringJob.AddOrUpdate(
		"database-backup-job",
		() => databaseBackup.BackupDatabaseToFileAsync(),
		Cron.Daily,
		new RecurringJobOptions
		{
			TimeZone = TimeZoneInfo.Utc
		}
	);
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<CurrentUserMiddleware>();
app.MapControllers();

app.Run();
