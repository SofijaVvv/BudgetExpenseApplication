namespace BudgetExpenseSystem.Repository.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<List<T>> All();
    Task<T?> GetById(int id);
    void Add(T entity);
    Task<bool> Delete(int id);
    void Update(T entity);
}