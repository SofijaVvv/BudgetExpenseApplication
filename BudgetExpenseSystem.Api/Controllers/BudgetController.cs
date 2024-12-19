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
public class BudgetController : ControllerBase
{
	private readonly IBudgetDomain _budgetDomain;

	public BudgetController(IBudgetDomain budgetDomain)
	{
		_budgetDomain = budgetDomain;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<BudgetResponse>))]
	public async Task<ActionResult<List<Budget>>> GetAllBudgets()
	{
		var budgets = await _budgetDomain.GetAllAsync();
		var result = budgets.ToResponse();

		return Ok(result);
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BudgetResponse))]
	public async Task<ActionResult> GetBudgetById([FromRoute] int id)
	{
		var budget = await _budgetDomain.GetByIdAsync(id);
		var result = budget.ToResponse();

		return Ok(result);
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(BudgetResponse))]
	public async Task<ActionResult> AddBudget([FromBody] BudgetRequest budgetRequest)
	{
		var result = budgetRequest.ToBudget();
		await _budgetDomain.AddAsync(result);

		return CreatedAtAction(nameof(GetBudgetById), new { id = result.Id }, result.ToResponse());
	}

	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<ActionResult> UpdateBudget(
		[FromRoute] int id,
		[FromBody] UpdateBudgetRequest updateBudgetRequest)
	{
		await _budgetDomain.Update(id, updateBudgetRequest);

		return NoContent();
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<ActionResult> DeleteBudget([FromRoute] int id)
	{
		await _budgetDomain.DeleteAsync(id);
		return NoContent();
	}
}