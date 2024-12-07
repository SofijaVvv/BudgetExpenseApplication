using BudgetExpenseSystem.Domain.Exceptions;
using BudgetExpenseSystem.Domain.Interfaces;
using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;

namespace BudgetExpenseSystem.Domain.Domains;

public class ScheduledTransactionDomain : IScheduledTransactionDomain
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IScheduledTransactionRepository _scheduledTransactionRepository;

	public ScheduledTransactionDomain(IUnitOfWork unitOfWork,
		IScheduledTransactionRepository scheduledTransactionRepository)
	{
		_unitOfWork = unitOfWork;
		_scheduledTransactionRepository = scheduledTransactionRepository;
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
		_scheduledTransactionRepository.AddAsync(scheduledTransaction);
		await _unitOfWork.SaveAsync();

		return scheduledTransaction;
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
