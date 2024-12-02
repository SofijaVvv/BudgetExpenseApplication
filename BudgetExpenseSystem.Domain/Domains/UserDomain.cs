using BudgetExpenseSystem.Domain.Exceptions;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;

namespace BudgetExpenseSystem.Domain.Domains;

public class UserDomain
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IUserRepository _userRepository;

	public UserDomain(IUnitOfWork unitOfWork, IUserRepository userRepository)
	{
		_unitOfWork = unitOfWork;
		_userRepository = userRepository;
	}

	public async Task<List<User>> GetAllAsync()
	{
		return await _userRepository.GetAllAsync();
	}

	public async Task<User> GetByIdAsync(int id)
	{
		var user = await _userRepository.GetByIdAsync(id);
		if (user == null) throw new NotFoundException($"User Id: {id} not found");

		return user;
	}

	public async Task<User> AddAsync(User user)
	{
		_userRepository.AddAsync(user);

		await _unitOfWork.SaveAsync();
		return user;
	}

	public async Task Update(User user)
	{
		_userRepository.Update(user);

		await _unitOfWork.SaveAsync();
	}

	public async Task DeleteAsync(int id)
	{
		var user = await _userRepository.GetByIdAsync(id);
		if (user == null) throw new NotFoundException($"User Id: {id} not found");

		await _userRepository.DeleteAsync(id);
		await _unitOfWork.SaveAsync();
	}
}
