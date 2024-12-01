using BudgetExpenseSystem.Domain.Domains;
using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Dto.Response;
using BudgetExpenseSystem.Model.Extentions;
using BudgetExpenseSystem.Model.Models;
using Microsoft.AspNetCore.Mvc;

namespace BudgetExpenseSystem.Api.Controllers;

[Route("api/[controller]s")]
[ApiController]
public class UserController : ControllerBase
{
	private readonly UserDomain _userDomain;

	public UserController(UserDomain userDomain)
	{
		_userDomain = userDomain;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserResponse>))]
	public async Task<ActionResult<List<User>>> GetAllUsers()
	{
		var users = await _userDomain.GetAllAsync();

		var result = users.ToResponse();
		return Ok(result);
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
	public async Task<IActionResult> GetUserById([FromRoute] int id)
	{
		var user = await _userDomain.GetByIdAsync(id);

		var result = user.ToResponse();
		return Ok(result);
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserResponse))]
	public async Task<ActionResult> AddUser([FromBody] UserRequest userRequest)
	{
		var result = userRequest.ToUser();

		await _userDomain.AddAsync(result);
		return CreatedAtAction(nameof(GetUserById), new { id = result.Id }, result);
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<ActionResult> DeleteUser([FromRoute] int id)
	{
		await _userDomain.DeleteAsync(id);
		return NoContent();
	}
}