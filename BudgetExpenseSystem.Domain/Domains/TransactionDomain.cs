using BudgetExpenseSystem.Domain.Exceptions;
using BudgetExpenseSystem.Domain.Interfaces;
using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Dto.Response;
using BudgetExpenseSystem.Model.Extentions;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;

namespace BudgetExpenseSystem.Domain.Domains;

public class TransactionDomain : ITransactionDomain
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ITransactionRepository _transactionRepository;

	public TransactionDomain(IUnitOfWork unitOfWork, ITransactionRepository transactionRepository)
	{
		_unitOfWork = unitOfWork;
		_transactionRepository = transactionRepository;
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

		var savedTransaction = await _transactionRepository.GetTransactionById(transaction.Id);
		if (savedTransaction == null)
			throw new NotFoundException($"The budget with Id {transaction.Id} could not be retrieved after saving.");

		return savedTransaction;
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