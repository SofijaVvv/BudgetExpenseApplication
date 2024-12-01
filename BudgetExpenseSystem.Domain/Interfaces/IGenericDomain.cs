namespace BudgetExpenseSystem.Domain.Interfaces;

public interface IGenericDomain<T> where T : class
{
	Task<List<T>> GetAllAsync();
	Task<T?> GetByIdAsync(int id);
	Task<T> AddAsync(T entity);
	Task Update(T entity);
	Task<bool> DeleteAsync(int id);
}