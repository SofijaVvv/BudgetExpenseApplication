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
public class AccountController : ControllerBase
{
	private readonly IAccountDomain _accountDomain;

	public AccountController(IAccountDomain accountDomain)
	{
		_accountDomain = accountDomain;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AccountResponse>))]
	public async Task<ActionResult> GetAllAccounts()
	{
		var roles = await _accountDomain.GetAllAsync();

		var result = roles.ToResponse();
		return Ok(result);
	}

	[HttpGet("{id}")]
	public async Task<ActionResult> GetAccountById(int id)
	{
		var role = await _accountDomain.GetByIdAsync(id);

		var result = role.ToResponse();
		return Ok(result);
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AccountResponse))]
	public async Task<ActionResult> AddAccount([FromBody] AccountRequest account)
	{
		var result = account.ToAccount();
		await _accountDomain.AddAsync(result);

		return CreatedAtAction(nameof(GetAccountById), new { id = result.Id }, result);
	}

	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<ActionResult> UpdateAccount(
		[FromRoute] int id,
		[FromBody] UpdateAccountRequest updateAccountRequest)
	{
		await _accountDomain.Update(id, updateAccountRequest);

		return NoContent();
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<ActionResult> DeleteAccount(int id)
	{
		await _accountDomain.DeleteAsync(id);
		return NoContent();
	}
}