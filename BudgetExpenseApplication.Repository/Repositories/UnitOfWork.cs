using BudgetExpenseApplication.Repository.Interfaces;

namespace BudgetExpenseApplication.Repository.Repositories;

public class UnitOfWork : IUnitOfWork
{
	private readonly ApplicationDbContext _context;


	public UnitOfWork(ApplicationDbContext context)
	{
		_context = context;
	}

	public IGenericRepository<T> GetRepository<T>() where T : class
	{
		return new GenericRepository<T>(_context);
	}

	public async Task SaveAsync()
	{
		await _context.SaveChangesAsync();
	}
}
