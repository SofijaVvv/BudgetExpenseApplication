using BudgetExpenseSystem.Domain.Domains;
using BudgetExpenseSystem.Domain.Interfaces;
using BudgetExpenseSystem.Repository.Interfaces;
using BudgetExpenseSystem.Repository.Repositories;

namespace BudgetExpenseSystem.Api.Extentions;

public static class ServiceExtensions
{
    public static void AddDomains(this IServiceCollection services)
    {
        services.AddScoped<IRoleDomain, RoleDomain>();
        services.AddScoped<IAccountDomain, AccountDomain>();
        services.AddScoped<IUserDomain, UserDomain>();
        services.AddScoped<IBudgetDomain, BudgetDomain>();
        services.AddScoped<IBudgetTypeDomain, BudgetTypeDomain>();
        services.AddScoped<ICategoryDomain, CategoryDomain>();
        services.AddScoped<INotificationDomain, NotificationDomain>();
        services.AddScoped<ITransactionDomain, TransactionDomain>();
        services.AddScoped<IScheduledTransactionDomain, ScheduledTransactionDomain>();
        services.AddScoped<IScheduledTransactionHandlerDomain, ScheduledTransactionHandlerDomain>();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IBudgetRepository, BudgetRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IBudgetTypeRepository, BudgetTypeRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IScheduledTransactionRepository, ScheduledTransactionRepository>();
    }


}
