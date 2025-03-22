using BudgetExpenseSystem.Domain.Exceptions;
using BudgetExpenseSystem.Domain.Interfaces;
using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseApplication.Repository.Interfaces;

namespace BudgetExpenseSystem.Domain.Domains;

public class ScheduledTransactionDomain : IScheduledTransactionDomain
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IScheduledTransactionRepository _scheduledTransactionRepository;
	private readonly IAccountRepository _accountRepository;
	private readonly ICategoryRepository _categoryRepository;
	private readonly IBudgetRepository _budgetRepository;

	public ScheduledTransactionDomain(
		IUnitOfWork unitOfWork,
		IScheduledTransactionRepository scheduledTransactionRepository,
		IAccountRepository accountRepository,
		ICategoryRepository categoryRepository,
		IBudgetRepository budgetRepository
	)
	{
		_unitOfWork = unitOfWork;
		_scheduledTransactionRepository = scheduledTransactionRepository;
		_accountRepository = accountRepository;
		_categoryRepository = categoryRepository;
		_budgetRepository = budgetRepository;
	}

	public async Task<List<ScheduledTransaction>> GetAllAsync()
	{
		return await _scheduledTransactionRepository.GetAllAsync();
	}

	public async Task<ScheduledTransaction> GetByIdAsync(int id)
	{
		var scheduledTransaction = await _scheduledTransactionRepository.GetByIdAsync(id);
		if (scheduledTransaction == null)
			throw new NotFoundException($"Scheduled transaction Id: {id} not found");

		return scheduledTransaction;
	}

	public async Task<ScheduledTransaction> AddAsync(ScheduledTransaction scheduledTransaction)
	{
		var account = await _accountRepository.GetByIdAsync(scheduledTransaction.AccountId);
		if (account == null) throw new NotFoundException($"Account Id: {scheduledTransaction.AccountId} not found");

		var category = await _categoryRepository.GetByIdAsync(scheduledTransaction.CategoryId);
		if (category == null) throw new NotFoundException($"Category Id: {scheduledTransaction.CategoryId} not found");

		var budget = await _budgetRepository.GetBudgetByIdAsync(scheduledTransaction.BudgetId);
		if (budget == null) throw new NotFoundException($"Budget Id: {scheduledTransaction.CategoryId} not found");

		_scheduledTransactionRepository.Add(scheduledTransaction);
		await _unitOfWork.SaveAsync();

		var savedScheduledTransaction = await _scheduledTransactionRepository.GetByIdAsync(scheduledTransaction.Id)
		                                ?? throw new Exception(
			                                "Something went wrong after saving scheduled transaction");

		return savedScheduledTransaction;
	}


	public async Task Update(
		int scheduledTransactionId,
		UpdateScheduleTransactionRequest updateScheduleTransactionRequest)
	{
		var scheduledTransaction = await _scheduledTransactionRepository.GetByIdAsync(scheduledTransactionId);
		if (scheduledTransaction == null)
			throw new NotFoundException($"Scheduled transaction Id: {scheduledTransactionId} not found");

		scheduledTransaction.CategoryId = updateScheduleTransactionRequest.CategoryId;
		scheduledTransaction.BudgetId = updateScheduleTransactionRequest.BudgetId;
		scheduledTransaction.Amount = updateScheduleTransactionRequest.Amount;
		scheduledTransaction.ScheduledDate = updateScheduleTransactionRequest.ScheduledDate;
		scheduledTransaction.IsRecurring = updateScheduleTransactionRequest.IsRecurring;

		await _unitOfWork.SaveAsync();
	}


	public async Task DeleteAsync(int id)
	{
		var scheduledTransaction = await _scheduledTransactionRepository.GetByIdAsync(id);
		if (scheduledTransaction == null) throw new NotFoundException($"Scheduled Transaction Id: {id} not found ");

		await _scheduledTransactionRepository.DeleteAsync(id);
		await _unitOfWork.SaveAsync();
	}
}
