using BudgetExpenseSystem.Domain.Domains;
using BudgetExpenseSystem.Domain.Interfaces;
using BudgetExpenseApplication.Repository.Interfaces;
using BudgetExpenseApplication.Repository.Repositories;
using BudgetExpenseApplication.Service;
using BudgetExpenseApplication.Service.Interfaces;
using BudgetExpenseApplication.Service.Mock;

namespace BudgetExpenseSystem.Api.Extentions;

public static class DependancyInjectionExtentions
{
    public static void AddDomains(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IRoleDomain, RoleDomain>();
        builder.Services.AddScoped<IAccountDomain, AccountDomain>();
        builder.Services.AddScoped<IUserDomain, UserDomain>();
        builder.Services.AddScoped<IBudgetDomain, BudgetDomain>();
        builder.Services.AddScoped<ICategoryDomain, CategoryDomain>();
        builder.Services.AddScoped<ITransactionDomain, TransactionDomain>();
        builder.Services.AddScoped<IScheduledTransactionDomain, ScheduledTransactionDomain>();
        builder.Services.AddScoped<IScheduledTransactionHandlerDomain, ScheduledTransactionHandlerDomain>();
    }

    public static void AddRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IBudgetRepository, BudgetRepository>();
        builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
        builder.Services.AddScoped<IAccountRepository, AccountRepository>();
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<IRoleRepository, RoleRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IScheduledTransactionRepository, ScheduledTransactionRepository>();
    }

    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
        if (builder.Environment.IsDevelopment())
            builder.Services.AddScoped<ICurrencyConversionService, MockCurrencyConversionService>();
        else
            builder.Services
                .AddScoped<ICurrencyConversionService, CurrencyConversionService>();
    }

}
