using BudgetExpenseApplication.Repository.Interfaces;
using BudgetExpenseApplication.Service.Interfaces;
using BudgetExpenseSystem.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BudgetExpenseApplication.Repository.Repositories;

public class BudgetRepository : GenericRepository<Budget>, IBudgetRepository
{
	private readonly ApplicationDbContext _context;
	private readonly ILogger<BudgetRepository> _logger;
	private readonly ICurrentUserService _currentUserService;

	public BudgetRepository(ApplicationDbContext context,
		ILogger<BudgetRepository> logger,
		ICurrentUserService currentUserService) : base(context)
	{
		_context = context;
		_logger = logger;
		_currentUserService = currentUserService;
	}

	public override async Task<List<Budget>> GetAllAsync()
	{
		var userId = _currentUserService.GetUserId();

		var budget = await _context.Budgets
			.Where(b => b.UserId == userId)
			.Include(b => b.Category)
			.Include(b => b.User)
			.ToListAsync();

		_logger.LogInformation("Fetched {count} budgets", budget.Count);

		return budget;
	}

	public override async Task<Budget?> GetByIdAsync(int id)
	{
		var budget = await _context.Budgets
			.Include(b => b.Category)
			.Include(b => b.User)
			.FirstOrDefaultAsync(b => b.Id == id);

		return budget;
	}
}
