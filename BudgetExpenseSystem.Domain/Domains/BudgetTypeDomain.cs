using BudgetExpenseSystem.Domain.Exceptions;
using BudgetExpenseSystem.Domain.Interfaces;
using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;

namespace BudgetExpenseSystem.Domain.Domains;

public class BudgetTypeDomain : IBudgetTypeDomain
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IBudgetTypeRepository _budgetTypeRepository;

	public BudgetTypeDomain(IUnitOfWork unitOfWork, IBudgetTypeRepository budgetTypeRepository)
	{
		_unitOfWork = unitOfWork;
		_budgetTypeRepository = budgetTypeRepository;
	}

	public async Task<List<BudgetType>> GetAllAsync()
	{
		return await _budgetTypeRepository.GetAllAsync();
	}


	public async Task<BudgetType> GetByIdAsync(int id)
	{
		var budgetType = await _budgetTypeRepository.GetByIdAsync(id);
		if (budgetType == null) throw new NotFoundException($"BudgetType Id: {id} not found");

		return budgetType;
	}


	public async Task<BudgetType> AddAsync(BudgetType budgetType)
	{
		_budgetTypeRepository.AddAsync(budgetType);

		await _unitOfWork.SaveAsync();
		return budgetType;
	}


	public async Task Update(int budgetTypeId, UpdateBudgetTypeRequest updateBudgetTypeRequest)
	{
		var budgetType = await _budgetTypeRepository.GetByIdAsync(budgetTypeId);
		if (budgetType == null) throw new NotFoundException($"BudgetType Id: {budgetTypeId} not found");

		budgetType.Name = updateBudgetTypeRequest.Name;

		await _unitOfWork.SaveAsync();
	}


	public async Task DeleteAsync(int id)
	{
		var budgetType = await _budgetTypeRepository.GetByIdAsync(id);
		if (budgetType == null) throw new NotFoundException($"BudgetType Id: {id} not found");

		await _budgetTypeRepository.DeleteAsync(id);
		await _unitOfWork.SaveAsync();
	}
}