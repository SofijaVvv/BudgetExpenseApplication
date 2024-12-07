using BudgetExpenseSystem.Domain.Domains;
using BudgetExpenseSystem.Domain.Interfaces;
using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Dto.Response;
using BudgetExpenseSystem.Model.Extentions;
using BudgetExpenseSystem.Model.Models;
using Microsoft.AspNetCore.Mvc;

namespace BudgetExpenseSystem.Api.Controllers;

[Route("api/[controller]s")]
[ApiController]
public class BudgetTypeController : ControllerBase
{
	private readonly IBudgetTypeDomain _budgetTypeDomain;

	public BudgetTypeController(IBudgetTypeDomain budgetTypeDomain)
	{
		_budgetTypeDomain = budgetTypeDomain;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<BudgetTypeResponse>))]
	public async Task<ActionResult<List<BudgetType>>> GetAllBudgetTypes()
	{
		var budgetTypes = await _budgetTypeDomain.GetAllAsync();
		var result = budgetTypes.ToResponse();

		return Ok(result);
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BudgetTypeResponse))]
	public async Task<ActionResult> GetBudgetTypeById([FromRoute] int id)
	{
		var budgetType = await _budgetTypeDomain.GetByIdAsync(id);
		var result = budgetType.ToResponse();

		return Ok(result);
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(BudgetTypeResponse))]
	public async Task<ActionResult> AddBudgetType([FromBody] BudgetTypeRequest budgetTypeRequest)
	{
		var result = budgetTypeRequest.ToBudgetType();
		await _budgetTypeDomain.AddAsync(result);

		return CreatedAtAction(nameof(GetBudgetTypeById), new { id = result.Id }, result.ToResponse());
	}

	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<ActionResult> UpdateBudgetType(
		[FromRoute] int id,
		[FromBody] UpdateBudgetTypeRequest updateBudgetTypeRequest)
	{
		await _budgetTypeDomain.Update(id, updateBudgetTypeRequest);

		return NoContent();
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<ActionResult> DeleteBudgetType([FromRoute] int id)
	{
		await _budgetTypeDomain.DeleteAsync(id);
		return NoContent();
	}
}