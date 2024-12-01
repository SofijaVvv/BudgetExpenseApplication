using BudgetExpenseSystem.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetExpenseSystem.Repository.Repositories;

public class TransactionRepository : GenericRepository<Transaction>
{
	private readonly ApplicationDbContext _context;

	public TransactionRepository(ApplicationDbContext context) : base(context)
	{
		_context = context;
	}

	public override async Task<List<Transaction>> GetAllAsync()
	{
		return await _context.Transactions
			.Include(t => t.Account)
			.Include(t => t.Category)
			.Include(t => t.Budget)
			.ToListAsync();
	}

	public override async Task<Transaction?> GetByIdAsync(int id)
	{
		return await _context.Transactions
			.Include(t => t.Account)
			.Include(t => t.Category)
			.Include(t => t.Budget)
			.FirstOrDefaultAsync(t => t.Id == id);
	}
}