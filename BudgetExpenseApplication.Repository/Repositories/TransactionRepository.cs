using BudgetExpenseApplication.Repository.Interfaces;
using BudgetExpenseApplication.Service.Interfaces;
using BudgetExpenseSystem.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetExpenseApplication.Repository.Repositories;

public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
{
	private readonly ApplicationDbContext _context;
	private readonly ICurrentUserService _currentUserService;
	public TransactionRepository(ApplicationDbContext context,
		ICurrentUserService currentUserService) : base(context)
	{
		_context = context;
		_currentUserService = currentUserService;
	}

	public override async Task<List<Transaction>> GetAllAsync()
	{
		var userId = _currentUserService.GetUserId();

		return await _context.Transactions
			.Include(t => t.Account)
			.Include(t => t.Budget)
			.Where(t => t.Account.UserId == userId)
			.ToListAsync();
	}

	public override async Task<Transaction?> GetByIdAsync(int id)
	{
		return await _context.Transactions
			.Include(t => t.Account)
			.Include(t => t.Budget)
			.FirstOrDefaultAsync(t => t.Id == id);
	}
}
