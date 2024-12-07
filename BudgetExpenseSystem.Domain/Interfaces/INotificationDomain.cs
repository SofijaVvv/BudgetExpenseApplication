using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseSystem.Domain.Interfaces;

public interface INotificationDomain
{
	Task<List<Notification>> GetAllAsync();
	Task<Notification> GetByIdAsync(int id);
	Task<Notification> AddAsync(Notification notification);
	Task Update(int notificationId, UpdateNotificationRequset updateNotificationRequset);
	Task DeleteAsync(int id);
}
