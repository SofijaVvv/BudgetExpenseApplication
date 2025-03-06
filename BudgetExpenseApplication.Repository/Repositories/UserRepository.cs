using System.Security.Claims;
using BudgetExpenseApplication.Repository.Interfaces;
using BudgetExpenseSystem.Model.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BudgetExpenseApplication.Repository.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
	private readonly ApplicationDbContext _context;
	private readonly IHttpContextAccessor _httpContextAccessor;


	public UserRepository(
		ApplicationDbContext context,
		IHttpContextAccessor httpContextAccessor) : base(context)
	{
		_context = context;
		_httpContextAccessor = httpContextAccessor;
	}

	public async Task<List<User>> GetAllUsersAsync()
	{
		return await _context.Users
			.Include(u => u.Role)
			.ToListAsync();
	}

	public async Task<User?> GetUserByIdAsync(int? id)
	{
		return await _context.Users
			.Include(u => u.Role)
			.FirstOrDefaultAsync(u => u.Id == id);
	}

	public override async Task<List<User>> GetAllAsync()
	{
		return await GetAllUsersAsync();
	}

	public async Task<User?> GetUserEmailAsync(string email)
	{
		return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
	}

	public int? GetCurrentUserId()
	{
		return int.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
	}


	public override async Task<User?> GetByIdAsync(int? id)
	{
		return await GetUserByIdAsync(id);
	}
}
