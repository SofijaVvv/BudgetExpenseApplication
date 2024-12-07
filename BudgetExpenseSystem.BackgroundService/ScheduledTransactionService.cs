using BudgetExpenseSystem.Background.Interface;
using BudgetExpenseSystem.Domain.Exceptions;
using BudgetExpenseSystem.Domain.Interfaces;
using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Models;
using Hangfire;
using Microsoft.Extensions.Logging;

namespace BudgetExpenseSystem.Background;

public class ScheduledTransactionService : IScheduledTransactionService
{
	private readonly IScheduledTransactionDomain _scheduledTransactionDomain;
	private readonly ITransactionDomain _transactionDomain;
	private readonly IAccountDomain _accountDomain;
	private readonly ILogger<ScheduledTransactionService> _logger;

	public ScheduledTransactionService(
		IScheduledTransactionDomain scheduledTransactionDomain,
		ILogger<ScheduledTransactionService> logger,
		ITransactionDomain transactionDomain,
		IAccountDomain accountDomain)
	{
		_scheduledTransactionDomain = scheduledTransactionDomain;
		_accountDomain = accountDomain;
		_transactionDomain = transactionDomain;
		_logger = logger;
	}


	public async Task ScheduleTransactionAsync(ScheduledTransaction transaction)
	{
		var addedTransaction = await _scheduledTransactionDomain.AddAsync(transaction);

		if (transaction.IsRecurring)
			RecurringJob.AddOrUpdate(
				addedTransaction.Id.ToString(),
				() => ProcessScheduledTransactionAsync(addedTransaction.Id),
				GetMonthlyCronExpression(addedTransaction.ScheduledDate),
				new RecurringJobOptions
				{
					TimeZone = TimeZoneInfo.Utc,
					QueueName = "default"
				}
			);
		else
			BackgroundJob.Schedule(
				() => ProcessScheduledTransactionAsync(addedTransaction.Id),
				addedTransaction.ScheduledDate
			);
	}


	private async Task ProcessScheduledTransactionAsync(int transactionId)
	{
		var scheduledTransaction = await _scheduledTransactionDomain.GetByIdAsync(transactionId);
		if (scheduledTransaction == null)
			throw new NotFoundException($"Scheduled transaction with ID {transactionId} was not found.");

		_logger.LogInformation(
			$"Processing scheduled transaction ID {transactionId} for amount {scheduledTransaction.Amount}.");

		await ProcessTransaction(scheduledTransaction);

		if (scheduledTransaction.IsRecurring)
		{
			RecurringJob.AddOrUpdate(
				scheduledTransaction.Id.ToString(),
				() => ProcessScheduledTransactionAsync(scheduledTransaction.Id),
				GetMonthlyCronExpression(scheduledTransaction.ScheduledDate),
				new RecurringJobOptions
				{
					TimeZone = TimeZoneInfo.Utc,
					QueueName = "default"
				}
			);
			_logger.LogInformation($"Scheduled transaction ID {transactionId} will repeat next month.");
		}
		else
		{
			_logger.LogInformation($"Scheduled transaction ID {transactionId} processed successfully, no repeat.");
		}
	}

	private async Task ProcessTransaction(ScheduledTransaction scheduledTransaction)
	{
		var account = await _accountDomain.GetByIdAsync(scheduledTransaction.AccountId);
		if (account == null)
			throw new NotFoundException($"Account with ID {scheduledTransaction.AccountId} not found.");

		if (account.Balance < scheduledTransaction.Amount)
			throw new InsufficientFundsException(
				$"Account with ID {account.Id} does not have enough funds for the scheduled transaction.");

		account.Balance -= scheduledTransaction.Amount;

		var updateAccountRequest = new UpdateAccountRequest
		{
			Balance = account.Balance
		};

		await _accountDomain.Update(account.Id, updateAccountRequest);

		var transaction = new Transaction
		{
			AccountId = account.Id,
			CategoryId = scheduledTransaction.CategoryId,
			BudgetId = scheduledTransaction.BudgetId,
			Amount = scheduledTransaction.Amount,
			TransactionDate = DateOnly.FromDateTime(DateTime.UtcNow)
		};

		await _transactionDomain.AddAsync(transaction);
		_logger.LogInformation(
			$"Processed transaction for scheduled transaction ID {scheduledTransaction.Id}. Account balance updated to {account.Balance}.");
	}


	public async Task UpdateScheduledTransactionAsync(int scheduledTransactionId,
		UpdateScheduleTransactionRequest updateScheduleTransaction)
	{
		var scheduledTransacton = await _scheduledTransactionDomain.GetByIdAsync(scheduledTransactionId);
		if (scheduledTransacton == null)
			throw new NotFoundException($"Scheduled transaction with ID {scheduledTransactionId} not found.");

		await _scheduledTransactionDomain.Update(scheduledTransactionId, updateScheduleTransaction);
		await ProcessScheduledTransactionAsync(scheduledTransactionId);

		if (updateScheduleTransaction.IsRecurring)
		{
			RecurringJob.AddOrUpdate(
				scheduledTransactionId.ToString(),
				() => ProcessScheduledTransactionAsync(scheduledTransactionId),
				GetMonthlyCronExpression(updateScheduleTransaction.ScheduledDate),
				new RecurringJobOptions
				{
					TimeZone = TimeZoneInfo.Utc,
					QueueName = "default"
				}
			);
			_logger.LogInformation(
				$"Scheduled transaction ID {scheduledTransactionId} is active and will repeat on the new schedule.");
		}
		else
		{
			RecurringJob.RemoveIfExists(scheduledTransactionId.ToString());
			BackgroundJob.Schedule(
				() => ProcessScheduledTransactionAsync(scheduledTransactionId),
				updateScheduleTransaction.ScheduledDate
			);
			_logger.LogInformation($"Scheduled transaction ID {scheduledTransactionId} has been updated.");
		}
	}

	public async Task DeleteScheduledTransactionAsync(int scheduledTransactionId)
	{
		var scheduledTransaction = await _scheduledTransactionDomain.GetByIdAsync(scheduledTransactionId);
		if (scheduledTransaction == null)
			throw new NotFoundException($"Scheduled transaction with ID {scheduledTransactionId} not found.");

		if (scheduledTransaction.IsRecurring)
		{
			RecurringJob.RemoveIfExists(scheduledTransactionId.ToString());
			_logger.LogInformation(
				$"Recurring job for scheduled transaction ID {scheduledTransactionId} has been removed.");
		}
		else
		{
			BackgroundJob.Delete(scheduledTransactionId.ToString());
			_logger.LogInformation(
				$"One time job for scheduled transaction ID {scheduledTransactionId} has been removed.");
		}

		await _scheduledTransactionDomain.DeleteAsync(scheduledTransactionId);
	}

	private string GetMonthlyCronExpression(DateTime scheduledDate)
	{
		return Cron.Monthly(scheduledDate.Day, scheduledDate.Hour, scheduledDate.Minute);
	}
}
