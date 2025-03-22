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
    public static void AddDomains(this IServiceCollection services)
    {
        services.AddScoped<IRoleDomain, RoleDomain>();
        services.AddScoped<IAccountDomain, AccountDomain>();
        services.AddScoped<IUserDomain, UserDomain>();
        services.AddScoped<IBudgetDomain, BudgetDomain>();
        services.AddScoped<ICategoryDomain, CategoryDomain>();
        services.AddScoped<ITransactionDomain, TransactionDomain>();
        services.AddScoped<IScheduledTransactionDomain, ScheduledTransactionDomain>();
        services.AddScoped<IScheduledTransactionHandlerDomain, ScheduledTransactionHandlerDomain>();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IBudgetRepository, BudgetRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IScheduledTransactionRepository, ScheduledTransactionRepository>();
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
