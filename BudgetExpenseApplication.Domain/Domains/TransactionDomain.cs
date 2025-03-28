using System.Security.Claims;
using BudgetExpenseSystem.Domain.Exceptions;
using BudgetExpenseSystem.Domain.Interfaces;
using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Enum;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseApplication.Repository.Interfaces;
using BudgetExpenseApplication.Service.Interfaces;
using BudgetExpenseApplication.WebSocket.Hub;
using BudgetExpenseSystem.Model.Extentions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace BudgetExpenseSystem.Domain.Domains;

public class TransactionDomain : ITransactionDomain
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentUserService _currentUserService;
	private readonly ITransactionRepository _transactionRepository;
	private readonly IAccountRepository _accountRepository;
	private readonly IBudgetDomain _budgetDomain;
	private readonly IHubContext<NotificationHub> _hubContext;
	private readonly ILogger<TransactionDomain> _logger;
	private readonly ICurrencyConversionService _currencyConversionService;

	public TransactionDomain(
		IUnitOfWork unitOfWork,
		ICurrentUserService currentUserService,
		ITransactionRepository transactionRepository,
		IAccountRepository accountRepository,
		IBudgetDomain budgetDomain,
		IHubContext<NotificationHub> hubContext,
		ILogger<TransactionDomain> logger,
		ICurrencyConversionService currencyConversionService
	)
	{
		_unitOfWork = unitOfWork;
		_currentUserService = currentUserService;
		_transactionRepository = transactionRepository;
		_accountRepository = accountRepository;
		_budgetDomain = budgetDomain;
		_hubContext = hubContext;
		_logger = logger;
		_currencyConversionService = currencyConversionService;
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

	public async Task<Transaction> AddAsync(TransactionRequest transactionRequest)
	{
		var userId = _currentUserService.GetUserId();
		if (userId == null)
			throw new UnauthorizedAccessException("User is unauthorised");

		var account = await _accountRepository.GetByUserIdAsync(userId);
		if (account == null) throw new NotFoundException($"Account for User Id: {userId} not found");

		var transaction = transactionRequest.ToTransaction(account.Id);

		if (!string.Equals(account.Currency, transaction.Currency))
			try
			{
				if (Enum.TryParse(transaction.Currency, true, out CurrencyCode fromCurrency) &&
				    Enum.TryParse(account.Currency, true, out CurrencyCode toCurrency))
				{
					var exchangeRates = await _currencyConversionService.GetExchangeRateAsync(fromCurrency, toCurrency);

					transaction.Amount *= exchangeRates;
					_logger.LogInformation($"Converted amount: {transaction.Amount}");
					transaction.Currency = account.Currency;
				}
				else
				{
					throw new Exception("Invalid currency code provided.");
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(
					$"Currency conversion failed for {transaction.Currency} to {account.Currency}: {ex.Message}");
				throw new Exception("Currency conversion failed.");
			}


		var message = await ProcessTransaction(transactionRequest, account);
		transaction.CreatedAt = DateTime.UtcNow;

		_transactionRepository.Add(transaction);
		await _unitOfWork.SaveAsync();

		await _hubContext.Clients.User(account.UserId.ToString())
			.SendAsync("ReceiveTransactionNotification", message);

		_logger.LogInformation($"Message being sent: {message}");
		_logger.LogInformation($"Sending notification to UserId: {account.UserId}");

		var savedTransaction = await _transactionRepository.GetByIdAsync(transaction.Id) ?? throw new Exception(
			"Something went wrong after saving  transaction");

		return savedTransaction;
	}


	private async Task<string> ProcessTransaction(TransactionRequest transactionRequest, Account account)
{
    if (string.IsNullOrEmpty(transactionRequest.TransactionType))
        throw new Exception("Transaction type must be provided.");

    if (transactionRequest.TransactionType == "Budget")
    {
        if (!transactionRequest.BudgetId.HasValue)
            throw new Exception("BudgetId must be provided for budget transactions.");

        if (transactionRequest.Amount >= 0)
	        throw new BadRequestException("Only negative amounts are allowed for budget.");

        var budget = await _budgetDomain.GetByIdAsync(transactionRequest.BudgetId.Value);

        if (budget.Amount < Math.Abs(transactionRequest.Amount))
            throw new InsufficientFundsException("Budget does not have enough funds for this transaction.");

        await _budgetDomain.UpdateBudgetFundsAsync(transactionRequest.BudgetId.Value, transactionRequest.Amount);
        account.Balance += transactionRequest.Amount;


        return $"You just recorded an expense of {transactionRequest.Amount}. Your remaining account balance is {account.Balance}.";
    }

    if (transactionRequest.TransactionType == "Account")
    {
	    if (transactionRequest.Amount < 0 && account.Balance < Math.Abs(transactionRequest.Amount))
		    throw new InsufficientFundsException("Account does not have enough funds for this transaction.");

	    account.Balance += transactionRequest.Amount;

	    return $"You just recorded a transaction of {transactionRequest.Amount}. Your account balance is {account.Balance}.";
    }

    return "Transaction processed successfully.";
}

	public async Task Update(int transactionId, UpdateTransactionRequest updateTransactionRequest)
	{
		var transaction = await _transactionRepository.GetByIdAsync(transactionId);
		if (transaction == null)
			throw new NotFoundException($"Transaction Id: {transactionId} not found");

		transaction.BudgetId = updateTransactionRequest.BudgetId;
		transaction.Amount = updateTransactionRequest.Amount;

		_transactionRepository.Update(transaction);
		await _unitOfWork.SaveAsync();
	}


	public async Task DeleteAsync(int id)
	{
		var transaction = await _transactionRepository.GetByIdAsync(id);
		if (transaction == null) throw new NotFoundException($"Transaction Id: {id} not found");

		 _transactionRepository.DeleteAsync(transaction);
		await _unitOfWork.SaveAsync();
	}
}
