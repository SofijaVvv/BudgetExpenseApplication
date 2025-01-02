using BudgetExpenseSystem.Domain.Interfaces;
using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Dto.Response;
using BudgetExpenseSystem.Model.Extentions;
using BudgetExpenseSystem.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetExpenseSystem.Api.Controllers;

[Route("api/[controller]s")]
[ApiController]
public class UserController : ControllerBase
{
	private readonly IUserDomain _userDomain;

	public UserController(IUserDomain userDomain)
	{
		_userDomain = userDomain;
	}

	[HttpGet]
	[Authorize(Policy = "AdminOnly")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserResponse>))]
	public async Task<ActionResult<List<User>>> GetAllUsers()
	{
		var users = await _userDomain.GetAllAsync();

		var result = users.ToResponse();
		return Ok(result);
	}

	[HttpGet("{id}")]
	[Authorize(Policy = "AdminOnly")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
	public async Task<IActionResult> GetUserById([FromRoute] int id)
	{
		var user = await _userDomain.GetByIdAsync(id);

		var result = user.ToResponse();
		return Ok(result);
	}

	[HttpPost("Register")]
	[AllowAnonymous]
	[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserResponse))]
	public async Task<ActionResult> RegisterUser([FromBody] UserRequest userRequest)
	{
		await _userDomain.RegisterUserAsync(userRequest);
		return Ok("User registered successfully.");
	}

	[HttpPost("Login")]
	public async Task<IActionResult> LoginUser([FromBody] UserRequest userRequest)
	{
		var token = await _userDomain.LoginUserAsync(userRequest.Email, userRequest.Password);

		return Ok(new TokenResponse { Token = token.Token });
	}

	[HttpDelete("{id}")]
	[Authorize(Policy = "AdminOnly")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<ActionResult> DeleteUser([FromRoute] int id)
	{
		await _userDomain.DeleteAsync(id);
		return NoContent();
	}
}
