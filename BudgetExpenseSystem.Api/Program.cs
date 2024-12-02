using BudgetExpenseSystem.Api.Filters;
using BudgetExpenseSystem.Domain.Domains;
using BudgetExpenseSystem.Domain.Interfaces;
using BudgetExpenseSystem.Repository;
using BudgetExpenseSystem.Repository.Interfaces;
using BudgetExpenseSystem.Repository.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers(options =>
	options.Filters.Add<GlobalExceptionFilter>()
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<RoleDomain>();
builder.Services.AddScoped<IAccountDomain, AccountDomain>();
builder.Services.AddScoped<UserDomain>();
builder.Services.AddScoped<IBudgetDomain, BudgetDomain>();
builder.Services.AddScoped<BudgetTypeDomain>();
builder.Services.AddScoped<CategoryDomain>();
builder.Services.AddScoped<NotificationDomain>();
builder.Services.AddScoped<TransactionDomain>();
builder.Services.AddScoped<IBudgetRepository, BudgetRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IBudgetTypeRepository, BudgetTypeRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddLogging();


var connectionString = builder.Configuration.GetConnectionString("ConnectionDefault")
                       ?? throw new Exception("Connection string 'ConnectionDefault' is not configured or is missing.");
var mySqlVersion = ServerVersion.Parse("10.4.28-mariadb");
builder.Services.AddDbContext<ApplicationDbContext>(options => { options.UseMySql(connectionString, mySqlVersion); });
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
