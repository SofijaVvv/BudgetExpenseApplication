using BudgetExpenseSystem.Domain.Exceptions;
using BudgetExpenseSystem.Domain.Interfaces;
using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;
using Hangfire;
using Hangfire.Storage;
using Microsoft.Extensions.Logging;

namespace BudgetExpenseSystem.Domain.Domains;

public class ScheduledTransactionDomain : IScheduledTransactionDomain
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IScheduledTransactionRepository _scheduledTransactionRepository;
	private readonly IAccountRepository _accountRepository;
	private readonly ICategoryRepository _categoryRepository;
	private readonly IBudgetRepository _budgetRepository;
	private readonly ITransactionDomain _transactionDomain;
	private readonly ILogger<ScheduledTransactionDomain> _logger;

	public ScheduledTransactionDomain(
		ILogger<ScheduledTransactionDomain> logger,
		ITransactionDomain transactionDomain,
		IUnitOfWork unitOfWork,
		IScheduledTransactionRepository scheduledTransactionRepository, IAccountRepository accountRepository,
		ICategoryRepository categoryRepository, IBudgetRepository budgetRepository

	)

	{
		_transactionDomain = transactionDomain;
		_logger = logger;
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

		_scheduledTransactionRepository.AddAsync(scheduledTransaction);
		await _unitOfWork.SaveAsync();

		var savedScheduledTransaction = await _scheduledTransactionRepository.GetByIdAsync(scheduledTransaction.Id)
		                                ?? throw new Exception(
			                                "Something went wrong after saving scheduled transaction");

		return savedScheduledTransaction;
	}

	public async Task ScheduleTransactionAsync(ScheduledTransaction transaction)
	{
		var addedTransaction = await AddAsync(transaction);

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

			await Update(addedTransaction.Id, new UpdateScheduleTransactionRequest()
			{
				CategoryId = addedTransaction.CategoryId,
				BudgetId = addedTransaction.BudgetId,
				Amount = addedTransaction.Amount,
				ScheduledDate = addedTransaction.ScheduledDate,
				IsRecurring = addedTransaction.IsRecurring
			});
		}
	}


	public async Task ProcessScheduledTransactionAsync(int transactionId)
	{
		var scheduledTransaction = await GetByIdAsync(transactionId);
		if (scheduledTransaction == null)
			throw new NotFoundException($"Scheduled transaction with ID {transactionId} was not found.");

		_logger.LogInformation(
			$"Processing scheduled transaction ID {transactionId} for amount {scheduledTransaction.Amount}.");

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
		var transaction = new Transaction
		{
			AccountId = scheduledTransaction.AccountId,
			CategoryId = scheduledTransaction.CategoryId,
			BudgetId = scheduledTransaction.BudgetId,
			Amount = scheduledTransaction.Amount,
			TransactionDate = DateOnly.FromDateTime(DateTime.UtcNow)
		};

		await _transactionDomain.AddAsync(transaction);
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

		scheduledTransaction.JobId = scheduledTransaction.JobId;

		await _unitOfWork.SaveAsync();
	}

	public async Task UpdateScheduledTransactionAsync(
		int scheduledTransactionId,
		UpdateScheduleTransactionRequest updateScheduleTransaction)
	{
		var scheduledTransaction = await GetByIdAsync(scheduledTransactionId)
		                           ?? throw new NotFoundException(
			                           $"Scheduled transaction with ID {scheduledTransactionId} not found.");

		if (scheduledTransaction.IsRecurring !=
		    updateScheduleTransaction.IsRecurring)
		{
			if (scheduledTransaction.IsRecurring)
			{
				List<RecurringJobDto> list;
				using (var connection = JobStorage.Current.GetConnection())
				{
					list = connection.GetRecurringJobs();
				}

				var job = list?.FirstOrDefault(j =>
					j.Id == scheduledTransaction.Id.ToString());
				if (job != null && !string.IsNullOrEmpty(job.LastJobId))
				{
					BackgroundJob.Delete(job.LastJobId);
					RecurringJob.RemoveIfExists(scheduledTransaction.Id.ToString());
				}

				_logger.LogInformation($"Removed recurring job with ID {scheduledTransaction.Id}");
			}
			else
			{
				BackgroundJob.Delete(scheduledTransaction.JobId);
				_logger.LogInformation($"Deleted one-time job with ID {scheduledTransaction.JobId}");
			}


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

		await Update(scheduledTransactionId, updateScheduleTransaction);
		_logger.LogInformation(
			$"STATUS {scheduledTransaction.IsRecurring} ");
	}

	public async Task DeleteAsync(int id)
	{
		var scheduledTransaction = await _scheduledTransactionRepository.GetByIdAsync(id);
		if (scheduledTransaction == null) throw new NotFoundException($"Scheduled Transaction Id: {id} not found ");

		await _scheduledTransactionRepository.DeleteAsync(id);
		await _unitOfWork.SaveAsync();
	}

	public async Task DeleteScheduledTransactionAsync(int scheduledTransactionId)
	{
		var scheduledTransaction = await GetByIdAsync(scheduledTransactionId);
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
			BackgroundJob.Delete(scheduledTransaction.JobId);
			_logger.LogInformation(
				$"One-time job for scheduled transaction ID {scheduledTransactionId} has been removed.");
		}

		await DeleteAsync(scheduledTransactionId);
	}

	public string GetMonthlyCronExpression(DateTime scheduledDate)
	{
		if (scheduledDate <= DateTime.UtcNow) throw new BadRequestException("ScheduledDate must be in the future.");
		return Cron.Monthly(scheduledDate.Day, scheduledDate.Hour, scheduledDate.Minute);
	}
}
