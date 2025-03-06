using BudgetExpenseApplication.Repository.Interfaces;
using BudgetExpenseSystem.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetExpenseApplication.Repository.Repositories;

public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
{
	private readonly ApplicationDbContext _context;

	public TransactionRepository(ApplicationDbContext context) : base(context)
	{
		_context = context;
	}

	public async Task<List<Transaction>> GetAllTransactionsAsync()
	{
		return await _context.Transactions
			.Include(t => t.Account)
			.Include(t => t.Category)
			.Include(t => t.Budget)
			.ToListAsync();
	}

	public async Task<Transaction?> GetTransactionById(int? id)
	{
		return await _context.Transactions
			.Include(t => t.Account)
			.Include(t => t.Category)
			.Include(t => t.Budget)
			.FirstOrDefaultAsync(t => t.Id == id);
	}

	public override async Task<List<Transaction>> GetAllAsync()
	{
		return await GetAllTransactionsAsync();
	}

	public override async Task<Transaction?> GetByIdAsync(int? id)
	{
		return await GetTransactionById(id);
	}
}
