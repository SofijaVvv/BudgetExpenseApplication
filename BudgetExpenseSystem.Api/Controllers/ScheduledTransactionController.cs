using BudgetExpenseSystem.Background.Interface;
using BudgetExpenseSystem.Domain.Interfaces;
using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Dto.Response;
using BudgetExpenseSystem.Model.Extentions;
using BudgetExpenseSystem.Model.Models;
using Microsoft.AspNetCore.Mvc;

namespace BudgetExpenseSystem.Api.Controllers;

[Route("api/[controller]s")]
[ApiController]
public class ScheduledTransactionController : ControllerBase
{
	private readonly IScheduledTransactionService _scheduledTransactionService;
	private readonly IScheduledTransactionDomain _scheduledTransactionDomain;

	public ScheduledTransactionController(IScheduledTransactionService scheduledTransactionService,
		IScheduledTransactionDomain scheduledTransactionDomain)
	{
		_scheduledTransactionService = scheduledTransactionService;
		_scheduledTransactionDomain = scheduledTransactionDomain;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ScheduleTransactionResponse>))]
	public async Task<ActionResult<List<Role>>> GetAllScheduledTransactions()
	{
		var scheduledTransactions = await _scheduledTransactionDomain.GetAllAsync();

		var result = scheduledTransactions.ToResponse();
		return Ok(result);
	}


	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ScheduleTransactionResponse))]
	public async Task<ActionResult> GetScheduledTransactionById([FromRoute] int id)
	{
		var scheduledTransaction = await _scheduledTransactionDomain.GetByIdAsync(id);

		var result = scheduledTransaction.ToResponse();
		return Ok(result);
	}


	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ScheduleTransactionResponse))]
	public async Task<ActionResult> AddScheduledTransaction(
		[FromBody] ScheduleTransactionRequest scheduleTransactionRequest)
	{
		var result = scheduleTransactionRequest.ToScheduledTransaction();
		await _scheduledTransactionService.ScheduleTransactionAsync(result);

		return CreatedAtAction(nameof(GetScheduledTransactionById), new { id = result.Id }, result.ToResponse());
	}

	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<ActionResult> UpdateRole(
		[FromRoute] int id,
		[FromBody] UpdateScheduleTransactionRequest updateScheduleTransactionRequest)
	{
		await _scheduledTransactionService.UpdateScheduledTransactionAsync(id, updateScheduleTransactionRequest);

		return NoContent();
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<ActionResult> DeleteScheduledTransaction([FromRoute] int id)
	{
		await _scheduledTransactionService.DeleteScheduledTransactionAsync(id);
		return NoContent();
	}
}
