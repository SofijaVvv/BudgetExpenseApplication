using BudgetExpenseSystem.Domain.Exceptions;
using BudgetExpenseSystem.Domain.Interfaces;
using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;

namespace BudgetExpenseSystem.Domain.Domains;

public class TransactionDomain : ITransactionDomain
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ITransactionRepository _transactionRepository;
	private readonly IAccountRepository _accountRepository;
	private readonly ICategoryRepository _categoryRepository;
	private readonly IBudgetDomain _budgetDomain;


	public TransactionDomain(IUnitOfWork unitOfWork, ITransactionRepository transactionRepository,
		IAccountRepository accountRepository, ICategoryRepository categoryRepository,
		IBudgetDomain budgetDomain)
	{
		_unitOfWork = unitOfWork;
		_transactionRepository = transactionRepository;
		_accountRepository = accountRepository;
		_categoryRepository = categoryRepository;
		_budgetDomain = budgetDomain;
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


	public async Task<Transaction?> AddAsync(Transaction transaction)
	{
		var account = await _accountRepository.GetByIdAsync(transaction.AccountId);
		if (account == null) throw new NotFoundException($"Account Id: {transaction.AccountId}");

		var category = await _categoryRepository.GetByIdAsync(transaction.CategoryId);
		if (category == null) throw new NotFoundException($"Category Id: {transaction.CategoryId}");

		var budget = await _budgetDomain.GetByIdAsync(transaction.BudgetId);
		if (budget == null) throw new NotFoundException($"Budget Id: {transaction.BudgetId} not found");

		await _budgetDomain.UpdateBudgetFundsAsync(transaction.BudgetId, transaction.Amount, transaction.CategoryId);

		if (account.Balance < transaction.Amount)
			throw new InsufficientFundsException("Account does not have enough funds for this transaction.");

		account.Balance -= transaction.Amount;
		_accountRepository.Update(account);

		_transactionRepository.AddAsync(transaction);
		await _unitOfWork.SaveAsync();

		var savedTransaction = await _transactionRepository.GetByIdAsync(transaction.Id);

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
