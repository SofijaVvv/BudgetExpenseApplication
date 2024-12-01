using BudgetExpenseSystem.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetExpenseSystem.Repository.Repositories;

public class UserRepository : GenericRepository<User>
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


	public override async Task<User?> GetByIdAsync(int id)
	{
		return await _context.Users
			.Include(u => u.Role)
			.FirstOrDefaultAsync(u => u.Id == id);
	}
}