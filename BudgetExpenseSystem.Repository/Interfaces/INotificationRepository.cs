using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseSystem.Repository.Interfaces;

public interface INotificationRepository : IGenericRepository<Notification>
{
	Task<List<Notification>> GetAllNotificationsAsync();
	Task<Notification?> GetNotificationByIdAsync(int id);
}
