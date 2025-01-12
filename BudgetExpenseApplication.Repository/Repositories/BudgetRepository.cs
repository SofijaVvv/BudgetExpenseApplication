using BudgetExpenseApplication.Repository.Interfaces;
using BudgetExpenseSystem.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BudgetExpenseApplication.Repository.Repositories;

public class BudgetRepository : GenericRepository<Budget>, IBudgetRepository
{
	private readonly ApplicationDbContext _context;
	private readonly ILogger<BudgetRepository> _logger;

	public BudgetRepository(ApplicationDbContext context, ILogger<BudgetRepository> logger) : base(context)
	{
		_context = context;
		_logger = logger;
	}

	public async Task<List<Budget>> GetAllBudgetsAsync()
	{
		var budget = await _context.Budgets
			.Include(b => b.Category)
			.Include(b => b.User)
			.ToListAsync();

		_logger.LogInformation("Fetched {count} budgets", budget.Count);

		return budget;
	}

	public async Task<Budget?> GetBudgetByIdAsync(int id)
	{
		var budget = await _context.Budgets
			.Include(b => b.Category)
			.Include(b => b.User)
			.FirstOrDefaultAsync(b => b.Id == id);

		return budget;
	}

	public override async Task<List<Budget>> GetAllAsync()
	{
		return await GetAllBudgetsAsync();
	}

	public override async Task<Budget?> GetByIdAsync(int id)
	{
		return await GetBudgetByIdAsync(id);
	}
}
