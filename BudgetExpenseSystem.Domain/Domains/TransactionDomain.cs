using BudgetExpenseSystem.Domain.Exceptions;
using BudgetExpenseSystem.Domain.Interfaces;
using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;

namespace BudgetExpenseSystem.Domain.Domains;

public class TransactionDomain
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IGenericRepository<Transaction> _transactionRepository;

	public TransactionDomain(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
		_transactionRepository = _unitOfWork.GetRepository<Transaction>();
	}


	public async Task<List<Transaction>> GetAllAsync()
	{
		return await _transactionRepository.GetAllAsync();
	}

	public async Task<Transaction> GetByIdAsync(int id)
	{
		var transaction = await _transactionRepository.GetByIdAsync(id);
		if (transaction == null) throw new NotFoundException($"Transaction Id: {id} not found");

		return transaction;
	}

	public async Task<Transaction> AddAsync(Transaction transaction)
	{
		_transactionRepository.AddAsync(transaction);

		await _unitOfWork.SaveAsync();
		return transaction;
	}

	public async Task Update(int transactionId, UpdateTransactionRequest updateTransactionRequest)
	{
		var transaction = await _transactionRepository.GetByIdAsync(transactionId);
		if (transaction == null)
			throw new NotFoundException($"Transaction Id: {transactionId} not found");

		transaction.CategoryId = updateTransactionRequest.CategoryId;
		transaction.BudgetId = updateTransactionRequest.BudgetId;
		transaction.Amount = updateTransactionRequest.Amount;

		_transactionRepository.Update(transaction);
		await _unitOfWork.SaveAsync();
	}

	public async Task DeleteAsync(int id)
	{
		var transaction = await _transactionRepository.GetByIdAsync(id);
		if (transaction == null) throw new NotFoundException($"Transaction Id: {id} not found");

		await _transactionRepository.DeleteAsync(id);
		await _unitOfWork.SaveAsync();
	}
}
