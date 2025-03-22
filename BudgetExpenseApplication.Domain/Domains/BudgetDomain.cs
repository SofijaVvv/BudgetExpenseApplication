using System.Security.Claims;
using BudgetExpenseSystem.Domain.Exceptions;
using BudgetExpenseSystem.Domain.Interfaces;
using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseApplication.Repository.Interfaces;
using BudgetExpenseApplication.Service.Interfaces;

namespace BudgetExpenseSystem.Domain.Domains;

public class BudgetDomain : IBudgetDomain
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentUserService _currentUserService;

	private readonly IBudgetRepository _budgetRepository;
	private readonly ICategoryRepository _categoryRepository;

	public BudgetDomain(
		IUnitOfWork unitOfWork,
		ICurrentUserService currentUserService,
		IBudgetRepository budgetRepository,
		ICategoryRepository categoryRepository
	)
	{
		_unitOfWork = unitOfWork;
		_currentUserService = currentUserService;
		_budgetRepository = budgetRepository;
		_categoryRepository = categoryRepository;
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

	public async Task UpdateBudgetFundsAsync(int budgetId, decimal amount)
	{
		var budget = await _budgetRepository.GetByIdAsync(budgetId);
		if (budget == null) throw new NotFoundException($"Budget Id: {budgetId} not found");


		if (amount < 0  && budget.Amount < Math.Abs(amount)) throw new InsufficientFundsException
		($"Budget with Id: {budgetId} does not have enough funds for the transaction.");

		budget.Amount += amount;
		await _unitOfWork.SaveAsync();
	}

	public async Task<Budget> AddAsync(Budget budget)
	{
		var userId = _currentUserService.GetUserId();
		if (userId is null) throw new UnauthorizedAccessException("User is unauthorised");

		var category = await _categoryRepository.GetByIdAsync(budget.CategoryId);
		if (category == null)
			throw new NotFoundException($"Category Id: {budget.CategoryId} not found");

		budget.CreatedAt = DateTime.UtcNow;
		_budgetRepository.Add(budget);
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

		budget.CreatedAt = DateTime.UtcNow;
		budget.Name = updateBudgetRequest.Name;
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
