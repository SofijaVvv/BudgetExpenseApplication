using BudgetExpenseSystem.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetExpenseSystem.Repository.Repositories;

public class NotificationRepository : GenericRepository<Notification>
{
    private readonly ApplicationDbContext _context;

    public NotificationRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    
    public override async Task<List<Notification>> GetAllAsync()
    {
        return await _context.Notifications
            .Include(n => n.User) 
            .ToListAsync();
    }
    
    
    public override async Task<Notification?> GetByIdAsync(int id)
    {
        return await _context.Notifications
            .Include(n => n.User)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

}