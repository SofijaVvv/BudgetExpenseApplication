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
public class TransactionController : ControllerBase
{
	private readonly ITransactionDomain _transactionDomain;

	public TransactionController(ITransactionDomain transactionDomain)
	{
		_transactionDomain = transactionDomain;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TransactionResponse>))]
	public async Task<ActionResult<List<Transaction>>> GetAllTransactions()
	{
		var transactions = await _transactionDomain.GetAllAsync();
		var result = transactions.ToResponse();

		return Ok(result);
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TransactionResponse))]
	public async Task<ActionResult> GetTransactionById([FromRoute] int id)
	{
		var transaction = await _transactionDomain.GetByIdAsync(id);
		var result = transaction.ToResponse();

		return Ok(result);
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TransactionResponse))]
	public async Task<ActionResult> AddTransaction([FromBody] TransactionRequest transactionRequest)
	{
		var result = await _transactionDomain.AddAsync(transactionRequest);

		return CreatedAtAction(nameof(GetTransactionById), new { id = result.Id }, result.ToResponse());
	}

	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<ActionResult> UpdateTransaction(
		[FromRoute] int id,
		[FromBody] UpdateTransactionRequest updateTransactionRequest)
	{
		await _transactionDomain.Update(id, updateTransactionRequest);

		return NoContent();
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<ActionResult> DeleteTransaction([FromRoute] int id)
	{
		await _transactionDomain.DeleteAsync(id);
		return NoContent();
	}
}
