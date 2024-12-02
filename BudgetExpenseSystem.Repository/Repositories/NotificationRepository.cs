using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BudgetExpenseSystem.Repository.Repositories;

public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
{
	private readonly ApplicationDbContext _context;

	public NotificationRepository(ApplicationDbContext context) : base(context)
	{
		_context = context;
	}

	public async Task<List<Notification>> GetAllNotificationsAsync()
	{
		return await _context.Notifications
			.Include(n => n.User)
			.ToListAsync();
	}

	public async Task<Notification?> GetNotificationByIdAsync(int id)
	{
		return await _context.Notifications
			.Include(n => n.User)
			.FirstOrDefaultAsync(u => u.Id == id);
	}

	public override async Task<List<Notification>> GetAllAsync()
	{
		return await GetAllNotificationsAsync();
	}


	public override async Task<Notification?> GetByIdAsync(int id)
	{
		return await GetNotificationByIdAsync(id);
	}
}