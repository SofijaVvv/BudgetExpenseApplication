using BudgetExpenseSystem.Domain.Exceptions;
using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;

namespace BudgetExpenseSystem.Domain.Domains;

public class BudgetDomain
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IGenericRepository<Budget> _budgetRepository;

	public BudgetDomain(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
		_budgetRepository = _unitOfWork.GetRepository<Budget>();
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
		return budget;
	}

	public async Task Update(int id, UpdateBudgetRequest budgetRequest)
	{
		var budget = await _budgetRepository.GetByIdAsync(id);
		if (budget == null) throw new NotFoundException($"Budget Id: {id} not found");

		await _unitOfWork.SaveAsync();
	}

	public async Task DeleteAsync(int id)
	{
		var budget = await _budgetRepository.GetByIdAsync(id);
		if(budget == null) throw new NotFoundException($"Budget Id: {id} not found");
		await _budgetRepository.DeleteAsync(id);
		await _unitOfWork.SaveAsync();
	}
}
