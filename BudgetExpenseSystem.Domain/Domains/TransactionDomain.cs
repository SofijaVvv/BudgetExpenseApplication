using BudgetExpenseSystem.Domain.Exceptions;
using BudgetExpenseSystem.Domain.Interfaces;
using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;
using BudgetExpenseSystem.WebSocket.Hub;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace BudgetExpenseSystem.Domain.Domains;

public class TransactionDomain : ITransactionDomain
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ITransactionRepository _transactionRepository;
	private readonly IAccountRepository _accountRepository;
	private readonly ICategoryRepository _categoryRepository;
	private readonly IBudgetDomain _budgetDomain;
	private readonly IHubContext<NotificationHub> _hubContext;
	private readonly ILogger<TransactionDomain> _logger;

	public TransactionDomain(
		IUnitOfWork unitOfWork,
		ITransactionRepository transactionRepository,
		IAccountRepository accountRepository,
		ICategoryRepository categoryRepository,
		IBudgetDomain budgetDomain,
		IHubContext<NotificationHub> hubContext,
		ILogger<TransactionDomain> logger
	)
	{
		_unitOfWork = unitOfWork;
		_transactionRepository = transactionRepository;
		_accountRepository = accountRepository;
		_categoryRepository = categoryRepository;
		_budgetDomain = budgetDomain;
		_hubContext = hubContext;
		_logger = logger;
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

		string message;

		if (transaction.Amount < 0)
		{
			await _budgetDomain.UpdateBudgetFundsAsync(transaction.BudgetId, transaction.Amount,
				transaction.CategoryId);

			if (account.Balance < Math.Abs(transaction.Amount))
				throw new InsufficientFundsException("Account does not have enough funds for this transaction.");

			account.Balance += transaction.Amount;

			message =
				$"You just recorded an expense of {transaction.Amount} for '{category.Name}'. Your remaining budget is {account.Balance}.";
		}
		else
		{
			account.Balance += transaction.Amount;
			message =
				$"You just recorded an income of {transaction.Amount} for '{category.Name}'. Your account balance is {account.Balance}.";
		}

		_transactionRepository.AddAsync(transaction);
		await _unitOfWork.SaveAsync();

		await _hubContext.Clients.User(account.UserId.ToString())
			.SendAsync("SendTransactionNotification", message);


		_logger.LogInformation($"Sending notification to UserId: {account.UserId}");


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
