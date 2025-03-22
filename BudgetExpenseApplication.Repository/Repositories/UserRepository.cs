using BudgetExpenseApplication.Repository.Interfaces;
using BudgetExpenseSystem.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetExpenseApplication.Repository.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
	private readonly ApplicationDbContext _context;


	public UserRepository(ApplicationDbContext context) : base(context)
	{
		_context = context;
	}

	public override async Task<List<User>> GetAllAsync()
	{
		return await _context.Users
			.Include(u => u.Role)
			.ToListAsync();
	}

	public async Task<User?> GetUserEmailAsync(string email)
	{
		return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
	}


	public override async Task<User?> GetByIdAsync(int? id)
	{
		return await _context.Users
			.Include(u => u.Role)
			.FirstOrDefaultAsync(u => u.Id == id);
	}
}
