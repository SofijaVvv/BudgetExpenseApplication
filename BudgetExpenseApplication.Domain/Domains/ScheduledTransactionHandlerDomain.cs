using BudgetExpenseSystem.Domain.Exceptions;
using BudgetExpenseSystem.Domain.Interfaces;
using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseApplication.Repository.Interfaces;
using BudgetExpenseApplication.WebSocket.Hub;
using Hangfire;
using Hangfire.Storage;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace BudgetExpenseSystem.Domain.Domains;

public class ScheduledTransactionHandlerDomain : IScheduledTransactionHandlerDomain
{
	private readonly IScheduledTransactionDomain _scheduledTransactionDomain;
	private readonly ILogger<ScheduledTransactionHandlerDomain> _logger;
	private readonly IUnitOfWork _unitOfWork;
	private readonly ITransactionDomain _transactionDomain;
	private readonly IAccountDomain _accountDomain;
	private readonly IHubContext<NotificationHub> _hubContext;

	public ScheduledTransactionHandlerDomain(IScheduledTransactionDomain scheduledTransactionDomain,
		ILogger<ScheduledTransactionHandlerDomain> logger, IUnitOfWork unitOfWork, ITransactionDomain transactionDomain,
		IAccountDomain accountDomain, IHubContext<NotificationHub> hubContext)
	{
		_scheduledTransactionDomain = scheduledTransactionDomain;
		_logger = logger;
		_unitOfWork = unitOfWork;
		_transactionDomain = transactionDomain;
		_accountDomain = accountDomain;
		_hubContext = hubContext;
	}


	public async Task NotifyUserAboutTransaction(ScheduledTransaction transaction)
	{
		var account = await _accountDomain.GetByIdAsync(transaction.AccountId);
		var userId = account.UserId;

		await _hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveTransactionNotification",
			$"Your scheduled recurring transaction of {transaction.Amount} in the category '{transaction.Category.Name}' will be processed in 24 hours.");
	}


	public async Task DeleteScheduledTransactionAsync(int scheduledTransactionId)
	{
		var scheduledTransaction =
			await _scheduledTransactionDomain.GetByIdAsync(scheduledTransactionId);

		if (scheduledTransaction.IsRecurring)
		{
			RecurringJob.RemoveIfExists(scheduledTransactionId.ToString());
			_logger.LogInformation(
				$"Recurring job for scheduled transaction ID {scheduledTransactionId} has been removed.");
		}
		else
		{
			BackgroundJob.Delete(scheduledTransaction.JobId);
			_logger.LogInformation(
				$"One-time job for scheduled transaction ID {scheduledTransactionId} has been removed.");
		}

		await _scheduledTransactionDomain.DeleteAsync(scheduledTransactionId);
	}

	public async Task ScheduleTransactionAsync(ScheduledTransaction transaction)
	{
		var addedTransaction = await _scheduledTransactionDomain.AddAsync(transaction);

		if (transaction.IsRecurring)
		{
			AddRecurringJob(addedTransaction);
		}
		else
		{
			var jobId = BackgroundJob.Schedule(
				() => ProcessScheduledTransactionAsync(addedTransaction.Id),
				addedTransaction.ScheduledDate
			);

			addedTransaction.JobId = jobId;
			await _unitOfWork.SaveAsync();
		}
	}


	public async Task ProcessScheduledTransactionAsync(int transactionId)
	{
		var scheduledTransaction = await _scheduledTransactionDomain.GetByIdAsync(transactionId);
		if (scheduledTransaction == null)
			throw new NotFoundException($"Scheduled transaction with ID {transactionId} was not found.");

		_logger.LogInformation(
			$"Processing scheduled transaction ID {transactionId} for amount {scheduledTransaction.Amount}.");

		if (scheduledTransaction.IsRecurring && scheduledTransaction.ScheduledDate > DateTime.UtcNow &&
		    scheduledTransaction.ScheduledDate <= DateTime.UtcNow.AddHours(24))
		{
			await NotifyUserAboutTransaction(scheduledTransaction);
		}


		await ProcessTransaction(scheduledTransaction);

		if (scheduledTransaction.IsRecurring)
		{
			AddRecurringJob(scheduledTransaction);
			_logger.LogInformation($"Scheduled transaction ID {transactionId} will repeat next month.");
		}
		else
		{
			_logger.LogInformation($"Scheduled transaction ID {transactionId} processed successfully, no repeat.");
		}
	}

	public void AddRecurringJob(ScheduledTransaction addedTransaction)
	{
		RecurringJob.AddOrUpdate(
			addedTransaction.Id.ToString(),
			() => ProcessScheduledTransactionAsync(addedTransaction.Id),
			GetMonthlyCronExpression(addedTransaction.ScheduledDate),
			new RecurringJobOptions
			{
				TimeZone = TimeZoneInfo.Utc
			}
		);
	}

	public async Task ProcessTransaction(ScheduledTransaction scheduledTransaction)
	{
		var transaction = new TransactionRequest
		{
			BudgetId = scheduledTransaction.BudgetId,
			Amount = scheduledTransaction.Amount,
		};

		await _transactionDomain.AddAsync(transaction);
	}

	private void RemoveOneTimeJob(string? jobId)
	{
		BackgroundJob.Delete(jobId);
		_logger.LogInformation($"Deleted one-time job with ID {jobId}");
	}

	private void RemoveRecurringJob(int scheduledTransactionId)
	{
		using var connection = JobStorage.Current.GetConnection();
		var recurringJobs = connection.GetRecurringJobs();
		var recurringJob = recurringJobs.FirstOrDefault(j => j.Id == scheduledTransactionId.ToString());

		if (recurringJob != null)
		{
			if (!string.IsNullOrEmpty(recurringJob.LastJobId))
			{
				BackgroundJob.Delete(recurringJob.LastJobId);
				_logger.LogInformation($"Deleted last executed job with ID {recurringJob.LastJobId}");
			}

			RecurringJob.RemoveIfExists(scheduledTransactionId.ToString());
			_logger.LogInformation($"Removed recurring job with ID {scheduledTransactionId}");
		}
		else
		{
			_logger.LogWarning($"No recurring job found with ID {scheduledTransactionId}");
		}
	}

	public async Task UpdateScheduledTransactionAsync(
		int scheduledTransactionId,
		UpdateScheduleTransactionRequest updateScheduleTransaction)
	{
		var scheduledTransaction = await _scheduledTransactionDomain.GetByIdAsync(scheduledTransactionId);

		if (scheduledTransaction.IsRecurring != updateScheduleTransaction.IsRecurring)
		{
			if (scheduledTransaction.IsRecurring)
				RemoveRecurringJob(scheduledTransaction.Id);
			else
				RemoveOneTimeJob(scheduledTransaction.JobId);


			if (updateScheduleTransaction.IsRecurring)
			{
				RecurringJob.AddOrUpdate(
					scheduledTransactionId.ToString(),
					() => ProcessScheduledTransactionAsync(scheduledTransactionId),
					GetMonthlyCronExpression(updateScheduleTransaction.ScheduledDate),
					new RecurringJobOptions
					{
						TimeZone = TimeZoneInfo.Utc
					}
				);
				_logger.LogInformation(
					$"Scheduled transaction ID {scheduledTransactionId} is now a recurring job.");
			}
			else
			{
				var jobId = BackgroundJob.Schedule(
					() => ProcessScheduledTransactionAsync(scheduledTransactionId),
					updateScheduleTransaction.ScheduledDate
				);
				scheduledTransaction.JobId = jobId;
			}
		}
		else
		{
			if (updateScheduleTransaction.IsRecurring)
			{
				RecurringJob.AddOrUpdate(
					scheduledTransactionId.ToString(),
					() => ProcessScheduledTransactionAsync(scheduledTransactionId),
					GetMonthlyCronExpression(updateScheduleTransaction.ScheduledDate),
					new RecurringJobOptions
					{
						TimeZone = TimeZoneInfo.Utc
					}
				);
			}
			else
			{
				BackgroundJob.Delete(scheduledTransaction.JobId);
				var jobId = BackgroundJob.Schedule(
					() => ProcessScheduledTransactionAsync(scheduledTransactionId),
					updateScheduleTransaction.ScheduledDate
				);
				scheduledTransaction.JobId = jobId;
			}
		}

		scheduledTransaction.CategoryId = updateScheduleTransaction.CategoryId;
		scheduledTransaction.BudgetId = updateScheduleTransaction.BudgetId;
		scheduledTransaction.Amount = updateScheduleTransaction.Amount;
		scheduledTransaction.ScheduledDate = updateScheduleTransaction.ScheduledDate;
		scheduledTransaction.IsRecurring = updateScheduleTransaction.IsRecurring;
		await _unitOfWork.SaveAsync();
	}

	public string GetMonthlyCronExpression(DateTime scheduledDate)
	{
		if (scheduledDate <= DateTime.UtcNow) throw new BadRequestException("ScheduledDate must be in the future.");
		return Cron.Monthly(scheduledDate.Day, scheduledDate.Hour, scheduledDate.Minute);
	}
}
