using System.Security.Claims;
using BudgetExpenseApplication.Repository.Interfaces;
using BudgetExpenseApplication.Service.Interfaces;
using BudgetExpenseSystem.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetExpenseApplication.Repository.Repositories;

public class AccountRepository : GenericRepository<Account>, IAccountRepository
{
	private readonly ApplicationDbContext _context;
	private readonly ICurrentUserService _currentUserService;

	public AccountRepository(ApplicationDbContext context, ICurrentUserService currentUserService) : base(context)
	{
		_currentUserService = currentUserService;
		_context = context;
	}


	public async Task<List<Account>> GetAllAccountsAsync()
	{
		var userIdClaim = _currentUserService.CurrentUser?.FindFirst(ClaimTypes.NameIdentifier);
		int userId = int.Parse(userIdClaim?.Value);

		var account = await _context.Accounts
			.Where(a => a.UserId == userId)
			.Include(a => a.User)
			.ToListAsync();

		return account;
	}

	public async Task<Account?> GetAccountByIdAsync(int? id)
	{
		var account = await _context.Accounts
			.Include(a => a.User)
			.FirstOrDefaultAsync(a => a.Id == id);

		return account;
	}

	public async Task<Account?> GetByUserIdAsync(int userId)
	{
		return await _context.Accounts.FirstOrDefaultAsync(a => a.UserId == userId);
	}

	public override async Task<List<Account>> GetAllAsync()
	{
		return await GetAllAccountsAsync();
	}

	public override async Task<Account?> GetByIdAsync(int? id)
	{
		return await GetAccountByIdAsync(id);
	}
}
