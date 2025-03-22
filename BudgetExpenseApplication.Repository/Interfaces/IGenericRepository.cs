namespace BudgetExpenseApplication.Repository.Interfaces;

public interface IGenericRepository<T> where T : class
{
	Task<List<T>> GetAllAsync();
	Task<T?> GetByIdAsync(int? id);
	void Add(T entity);
	Task<bool> DeleteAsync(int id);
	void Update(T entity);
}