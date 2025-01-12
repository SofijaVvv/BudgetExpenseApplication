using BudgetExpenseSystem.Domain.Interfaces;
using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Dto.Response;
using BudgetExpenseSystem.Model.Extentions;
using BudgetExpenseSystem.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetExpenseSystem.Api.Controllers;

[Authorize(Policy = "UserOnly")]
[Route("api/[controller]s")]
[ApiController]
public class ScheduledTransactionController : ControllerBase
{
	private readonly IScheduledTransactionDomain _scheduledTransactionDomain;
	private readonly IScheduledTransactionHandlerDomain _scheduledTransactionHandlerDomain;

	public ScheduledTransactionController(IScheduledTransactionDomain scheduledTransactionDomain,
		IScheduledTransactionHandlerDomain scheduledTransactionHandlerDomain)
	{
		_scheduledTransactionDomain = scheduledTransactionDomain;
		_scheduledTransactionHandlerDomain = scheduledTransactionHandlerDomain;
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
		await _scheduledTransactionHandlerDomain.ScheduleTransactionAsync(result);

		return CreatedAtAction(nameof(GetScheduledTransactionById), new { id = result.Id }, result.ToResponse());
	}

	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<ActionResult> UpdateRole(
		[FromRoute] int id,
		[FromBody] UpdateScheduleTransactionRequest updateScheduleTransactionRequest)
	{
		await _scheduledTransactionHandlerDomain.UpdateScheduledTransactionAsync(id, updateScheduleTransactionRequest);

		return NoContent();
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<ActionResult> DeleteScheduledTransaction([FromRoute] int id)
	{
		await _scheduledTransactionHandlerDomain.DeleteScheduledTransactionAsync(id);
		return NoContent();
	}
}
