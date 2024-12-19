using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BudgetExpenseSystem.Repository.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
	private readonly ApplicationDbContext _context;

	public UserRepository(ApplicationDbContext context) : base(context)
	{
		_context = context;
	}

	public async Task<List<User>> GetAllUsersAsync()
	{
		return await _context.Users
			.Include(u => u.Role)
			.ToListAsync();
	}

	public async Task<User?> GetUserByIdAsync(int id)
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


	public override async Task<User?> GetByIdAsync(int id)
	{
		return await GetUserByIdAsync(id);
	}
}
