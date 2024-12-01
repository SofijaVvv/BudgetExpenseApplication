using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseSystem.Repository.Interfaces;

public interface IUnitOfWork
{
	IGenericRepository<T> GetRepository<T>() where T : class;

	Task SaveAsync();
}