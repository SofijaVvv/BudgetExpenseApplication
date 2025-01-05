using BudgetExpenseSystem.Api.Controllers;
using BudgetExpenseSystem.Domain.Interfaces;
using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Dto.Response;
using BudgetExpenseSystem.Model.Extentions;
using BudgetExpenseSystem.Model.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BudgetExpenseSystem.Tests.Controllers;

[TestFixture]
public class AccountControllerTests
{
	private readonly Mock<IAccountDomain> _mockAccountDomain;
	private readonly AccountController _accountController;

	public AccountControllerTests()
	{
		_mockAccountDomain = new Mock<IAccountDomain>();
		_accountController = new AccountController(_mockAccountDomain.Object);
	}


	[Test]
	public async Task GetAllAccounts_ShouldReturnOk_WithAccountList()
	{
		// Arrange
		var account = new List<Account>
		{
			new() { Id = 1, Balance = 200 },
			new() { Id = 2, Balance = 200 }
		};

		_mockAccountDomain.Setup(domain => domain.GetAllAsync()).ReturnsAsync(account);

		// Act
		var result = await _accountController.GetAllAccounts();

		// Assert
		var okResult = result as OkObjectResult;
		Assert.That(okResult, Is.Not.Null);
		var returnedAccounts = okResult.Value as List<AccountResponse>;
		Assert.That(returnedAccounts, Is.Not.Null);
		Assert.That(returnedAccounts.Count, Is.EqualTo(account.Count));
	}

	[Test]
	public async Task GetAccountById_ShouldReturnOk_WhenAccountExists()
	{
		// Arrange
		var account = new Account { Id = 1, Balance = 200 };

		_mockAccountDomain.Setup(domain => domain.GetByIdAsync(1)).ReturnsAsync(account);

		// Act
		var result = await _accountController.GetAccountById(1);

		// Assert
		var okResult = result as OkObjectResult;
		Assert.That(okResult, Is.Not.Null);

		var returnedAccount = okResult.Value as AccountResponse;
		Assert.That(returnedAccount, Is.Not.Null);
		Assert.That(returnedAccount.Id, Is.EqualTo(account.Id));
	}


	[Test]
	public async Task AddAccount_ShouldReturnCreated_WhenAccountIsAdded()
	{
		// Arrange
		var accountRequest = new AccountRequest { UserId = 2, Balance = 1000, Currency = "EUR" };
		var account = accountRequest.ToAccount();

		_mockAccountDomain.Setup(domain => domain.AddAsync(account));

		// Act
		var result = await _accountController.AddAccount(accountRequest);

		// Assert
		var createdResult = result as CreatedAtActionResult;
		Assert.That(createdResult, Is.Not.Null);
		var returnedAccount = createdResult.Value as AccountResponse;
		Assert.That(returnedAccount, Is.Not.Null);
		Assert.That(returnedAccount.Id, Is.EqualTo(account.Id));
		Assert.That(createdResult.ActionName, Is.EqualTo(nameof(_accountController.GetAccountById)));
	}

	[Test]
	public async Task UpdateAccount_ShouldReturnNoContent_WhenAccountIsUpdated()
	{
		// Arrange
		var accountId = 1;
		var updateAccountRequest = new UpdateAccountRequest
			{ Balance = 1500, Currency = "USD" };
		_mockAccountDomain.Setup(domain => domain.Update(accountId, updateAccountRequest)).Returns(Task.CompletedTask);

		// Act
		var result = await _accountController.UpdateAccount(accountId, updateAccountRequest);

		// Assert
		Assert.That(result, Is.InstanceOf<NoContentResult>());
	}

	[Test]
	public async Task DeleteAccount_ShouldReturnNoContent_WhenAccountIsDeleted()
	{
		// Arrange
		var accountId = 1; // Use a valid account ID
		_mockAccountDomain.Setup(domain => domain.DeleteAsync(accountId)).Returns(Task.CompletedTask);

		// Act
		var result = await _accountController.DeleteAccount(accountId);

		// Assert
		Assert.That(result, Is.InstanceOf<NoContentResult>());
	}
}