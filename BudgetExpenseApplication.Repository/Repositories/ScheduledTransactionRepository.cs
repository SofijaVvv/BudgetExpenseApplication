using BudgetExpenseApplication.Repository.Interfaces;
using BudgetExpenseSystem.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetExpenseApplication.Repository.Repositories;

public class ScheduledTransactionRepository : GenericRepository<ScheduledTransaction>, IScheduledTransactionRepository
{
	private readonly ApplicationDbContext _context;

	public ScheduledTransactionRepository(ApplicationDbContext context) : base(context)
	{
		_context = context;
	}


	public async Task<List<ScheduledTransaction>> GetAllScheduledTransactionsAsync()
	{
		return await _context.ScheduledTransactions
			.Include(t => t.Account)
			.Include(t => t.Category)
			.Include(t => t.Budget)
			.ToListAsync();
	}

	public async Task<ScheduledTransaction?> GetScheduledTransactionByIdAsync(int? id)
	{
		return await _context.ScheduledTransactions
			.Include(t => t.Account)
			.Include(t => t.Category)
			.Include(t => t.Budget)
			.FirstOrDefaultAsync(t => t.Id == id);
	}

	public override async Task<List<ScheduledTransaction>> GetAllAsync()
	{
		return await GetAllScheduledTransactionsAsync();
	}

	public override async Task<ScheduledTransaction?> GetByIdAsync(int? id)
	{
		return await GetScheduledTransactionByIdAsync(id);
	}
}
