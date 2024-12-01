using BudgetExpenseSystem.Domain.Domains;
using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Dto.Response;
using BudgetExpenseSystem.Model.Extentions;
using BudgetExpenseSystem.Model.Models;
using Microsoft.AspNetCore.Mvc;

namespace BudgetExpenseSystem.Api.Controllers;

[Route("api/[controller]s")]
[ApiController]
public class RoleController : ControllerBase
{
	private readonly RoleDomain _roleDomain;

	public RoleController(RoleDomain roleDomain)
	{
		_roleDomain = roleDomain;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<RoleResponse>))]
	public async Task<ActionResult<List<Role>>> GetAllRoles()
	{
		var roles = await _roleDomain.GetAllAsync();

		var result = roles.ToResponse();
		return Ok(result);
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoleResponse))]
	public async Task<ActionResult> GetRoleById([FromRoute] int id)
	{
		var role = await _roleDomain.GetByIdAsync(id);

		var result = role.ToResponse();
		return Ok(result);
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RoleResponse))]
	public async Task<ActionResult> AddRole([FromBody] RoleRequest roleRequest)
	{
		var result = roleRequest.ToRole();
		await _roleDomain.AddAsync(result);

		return CreatedAtAction(nameof(GetRoleById), new { id = result.Id }, result);
	}

	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<ActionResult> UpdateRole(
		[FromRoute] int id,
		[FromBody] UpdateRoleRequest updateRoleRequest)
	{
		await _roleDomain.Update(id, updateRoleRequest);

		return NoContent();
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<ActionResult> DeleteRole([FromRoute] int id)
	{
		await _roleDomain.DeleteAsync(id);
		return NoContent();
	}
}
