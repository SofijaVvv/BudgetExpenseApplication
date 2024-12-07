using BudgetExpenseSystem.Domain.Exceptions;
using BudgetExpenseSystem.Domain.Interfaces;
using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Dto.Response;
using BudgetExpenseSystem.Model.Extentions;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;

namespace BudgetExpenseSystem.Domain.Domains;

public class BudgetDomain : IBudgetDomain
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IBudgetRepository _budgetRepository;

	public BudgetDomain(IUnitOfWork unitOfWork, IBudgetRepository budgetRepository)
	{
		_unitOfWork = unitOfWork;
		_budgetRepository = budgetRepository;
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

	public async Task<Budget> AddAsync(Budget budget)
	{
		_budgetRepository.AddAsync(budget);
		await _unitOfWork.SaveAsync();

		var savedBudget = await _budgetRepository.GetBudgetByIdAsync(budget.Id);
		if (savedBudget == null)
			throw new NotFoundException($"The budget with Id {budget.Id} could not be retrieved after saving.");

		return savedBudget;
	}

	public async Task Update(int id, UpdateBudgetRequest updateBudgetRequest)
	{
		var budget = await _budgetRepository.GetByIdAsync(id);
		if (budget == null) throw new NotFoundException($"Budget Id: {id} not found");

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
