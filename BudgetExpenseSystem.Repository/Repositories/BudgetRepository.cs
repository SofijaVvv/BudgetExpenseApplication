using BudgetExpenseSystem.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetExpenseSystem.Repository.Repositories;

public class BudgetRepository : GenericRepository<Budget>
{
    private readonly ApplicationDbContext _context;

    public BudgetRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<List<Budget>> All()
    {
        return await _context.Budgets
            .Include(b => b.User)
            .Include(b => b.Category)
            .Include(b => b.BudgetType)
            .ToListAsync();
    }
    
    public override async Task<Budget?> GetById(int id)
    {
        return await _context.Budgets
            .Include(b => b.User)
            .Include(b => b.Category)
            .Include(b => b.BudgetType)
            .FirstOrDefaultAsync(b => b.Id == id);
    }
}