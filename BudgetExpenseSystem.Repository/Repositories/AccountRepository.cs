using BudgetExpenseSystem.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetExpenseSystem.Repository.Repositories;

public class AccountRepository : GenericRepository<Account>
{
    private readonly ApplicationDbContext _context;

    public AccountRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }


    public override async Task<List<Account>> All()
    {
        return await _context.Accounts
            .Include(a => a.User)
            .ToListAsync();
    }


    public override async Task<Account?> GetById(int id)
    {
        return await _context.Accounts
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.Id == id);
    }
}