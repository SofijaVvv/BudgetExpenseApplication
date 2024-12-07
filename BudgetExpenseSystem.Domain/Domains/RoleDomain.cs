using BudgetExpenseSystem.Domain.Exceptions;
using BudgetExpenseSystem.Domain.Interfaces;
using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;

namespace BudgetExpenseSystem.Domain.Domains;

public class RoleDomain : IRoleDomain
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IRoleRepository _roleRepository;

	public RoleDomain(IUnitOfWork unitOfWork, IRoleRepository roleRepository)
	{
		_unitOfWork = unitOfWork;
		_roleRepository = roleRepository;
	}

	public async Task<List<Role>> GetAllAsync()
	{
		return await _roleRepository.GetAllAsync();
	}


	public async Task<Role> GetByIdAsync(int id)
	{
		var role = await _roleRepository.GetByIdAsync(id);
		if (role == null) throw new NotFoundException($"Role Id: {id} not found");

		return role;
	}


	public async Task<Role> AddAsync(Role role)
	{
		_roleRepository.AddAsync(role);

		await _unitOfWork.SaveAsync();
		return role;
	}


	public async Task Update(int roleId, UpdateRoleRequest updateRoleRequest)
	{
		var role = await _roleRepository.GetByIdAsync(roleId);
		if (role == null) throw new NotFoundException($"Role Id: {roleId} not found");

		role.Name = updateRoleRequest.Name;

		await _unitOfWork.SaveAsync();
	}


	public async Task DeleteAsync(int id)
	{
		var role = await _roleRepository.GetByIdAsync(id);
		if (role == null) throw new NotFoundException($"Role Id: {id} not found");

		await _roleRepository.DeleteAsync(id);
		await _unitOfWork.SaveAsync();
	}
}