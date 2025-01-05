using BudgetExpenseSystem.Domain.Exceptions;
using BudgetExpenseSystem.Domain.Interfaces;
using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;

namespace BudgetExpenseSystem.Domain.Domains;

public class BudgetDomain : IBudgetDomain
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IBudgetRepository _budgetRepository;
	private readonly ICategoryRepository _categoryRepository;
	private readonly IBudgetTypeRepository _budgetTypeRepository;

	public BudgetDomain(
		IUnitOfWork unitOfWork,
		IBudgetRepository budgetRepository,
		ICategoryRepository categoryRepository,
		IBudgetTypeRepository budgetTypeRepository
	)
	{
		_unitOfWork = unitOfWork;
		_budgetRepository = budgetRepository;
		_categoryRepository = categoryRepository;
		_budgetTypeRepository = budgetTypeRepository;
	}

	public async Task<List<Budget>> GetAllAsync()
	{
		return await _budgetRepository.GetAllAsync();
	}

	public async Task<Budget> GetByIdAsync(int id)
	{
		var budget = await _budgetRepository.GetByIdAsync(id);
		if (budget == null) throw new NotFoundException($"Budget Id: {id} not found");

		return budget;
	}

	public async Task UpdateBudgetFundsAsync(int budgetId, decimal amount, int categoryId)
	{
		var budget = await _budgetRepository.GetByIdAsync(budgetId);
		if (budget == null) throw new NotFoundException($"Budget Id: {budgetId} not found");


		if (budget.CategoryId != categoryId)
			throw new BadRequestException(
				$"The category of the transaction does not match the category of the budget.");

		if (amount < 0)
			if (budget.Amount < Math.Abs(amount))
				throw new InsufficientFundsException(
					$"Budget with Id: {budgetId} does not have enough funds for the transaction.");

		budget.Amount += amount;
		_budgetRepository.Update(budget);
		await _unitOfWork.SaveAsync();
	}

	public async Task<Budget> AddAsync(Budget budget)
	{
		var category = await _categoryRepository.GetByIdAsync(budget.CategoryId);
		if (category == null)
			throw new NotFoundException($"Category Id: {budget.CategoryId} not found");

		var budgetType = await _budgetTypeRepository.GetByIdAsync(budget.BudgetTypeId);
		if (budgetType == null)
			throw new NotFoundException($"budgetType Id: {budget.CategoryId} not found");

		_budgetRepository.AddAsync(budget);
		await _unitOfWork.SaveAsync();

		var savedBudget = await _budgetRepository.GetByIdAsync(budget.Id) ?? throw new Exception(
			"Something went wrong after saving budget");

		return savedBudget;
	}

	public async Task Update(int id, UpdateBudgetRequest updateBudgetRequest)
	{
		var budget = await _budgetRepository.GetByIdAsync(id);
		if (budget == null) throw new NotFoundException($"Budget Id: {id} not found");

		var category = await _categoryRepository.GetByIdAsync(updateBudgetRequest.CategoryId);
		if (category == null) throw new NotFoundException($"Category Id: {updateBudgetRequest.CategoryId} not found");

		var budgetType = await _budgetTypeRepository.GetByIdAsync(updateBudgetRequest.BudgetTypeId);
		if (budgetType == null)
			throw new NotFoundException($"budgetType Id: {updateBudgetRequest.BudgetTypeId} not found");

		budget.BudgetTypeId = updateBudgetRequest.BudgetTypeId;
		budget.CategoryId = updateBudgetRequest.CategoryId;
		budget.Amount = updateBudgetRequest.Amount;

		await _unitOfWork.SaveAsync();
	}

	public async Task DeleteAsync(int id)
	{
		var budget = await _budgetRepository.GetByIdAsync(id);
		if (budget == null) throw new NotFoundException($"Budget Id: {id} not found");

		await _budgetRepository.DeleteAsync(id);
		await _unitOfWork.SaveAsync();
	}
}
