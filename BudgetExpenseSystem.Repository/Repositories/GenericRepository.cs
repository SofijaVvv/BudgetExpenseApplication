using BudgetExpenseSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BudgetExpenseSystem.Repository.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
	private readonly ApplicationDbContext _context;
	private readonly DbSet<T> _dbSet;

	public GenericRepository(ApplicationDbContext context)
	{
		_context = context;
		_dbSet = _context.Set<T>();
	}


	public virtual async Task<List<T>> GetAllAsync()
	{
		return await _dbSet.ToListAsync();
	}

	public virtual async Task<T?> GetByIdAsync(int id)
	{
		return await _dbSet.FindAsync(id);
	}

	public virtual void AddAsync(T entity)
	{
		_dbSet.Add(entity);
	}

	public virtual async Task<bool> DeleteAsync(int id)
	{
		var entity = await GetByIdAsync(id);
		if (entity == null) return false;

		_dbSet.Remove(entity);
		return true;
	}

	public virtual void Update(T entity)
	{
		_dbSet.Update(entity);
	}
}
