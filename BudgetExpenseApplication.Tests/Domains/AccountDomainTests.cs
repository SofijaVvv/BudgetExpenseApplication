using BudgetExpenseSystem.Domain.Domains;
using BudgetExpenseSystem.Domain.Exceptions;
using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseApplication.Repository.Interfaces;
using BudgetExpenseApplication.Service.Interfaces;
using Moq;

namespace BudgetExpenseApplication.Tests.Domains;

[TestFixture]
public class AccountDomainTests
{
	private readonly Mock<IAccountRepository> _mockAccountRepository;
	private readonly Mock<IUnitOfWork> _mockUnitOfWork;
	private readonly AccountDomain _accountDomain;
	private readonly Mock<ICurrentUserService> _mockCurrentUserService;

	public AccountDomainTests()
	{
		_mockAccountRepository = new Mock<IAccountRepository>();
		_mockUnitOfWork = new Mock<IUnitOfWork>();
		_accountDomain = new AccountDomain(_mockUnitOfWork.Object, _mockAccountRepository.Object, _mockCurrentUserService?.Object);
	}

	[Test]
	public async Task GetAllAsync_ShouldReturnAccounts()
	{
		// Arrange
		var accounts = new List<Account>
		{
			new() { Id = 1, UserId = 8, Balance = 100 },
			new() { Id = 2, UserId = 4, Balance = 1000 }
		};
		_mockAccountRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(accounts);

		// Act
		var result = await _accountDomain.GetAllAsync();

		// Assert
		Assert.That(result.Count, Is.EqualTo(2));
		Assert.That(result[0].Balance, Is.EqualTo(100));
		Assert.That(result[1].Balance, Is.EqualTo(1000));
	}

	[Test]
	public async Task GetByIdAsync_ShouldReturnAccountById()
	{
		var account = new Account { Id = 1, UserId = 8, Balance = 100 };

		_mockAccountRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(account);

		var result = await _accountDomain.GetByIdAsync(1);

		Assert.That(result, Is.Not.Null);
		Assert.That(result.Id, Is.EqualTo(1));
		Assert.That(result.Balance, Is.EqualTo(100));
	}

	[Test]
	public void GetByIdAsync_ShouldThrowNotFoundException_WhenAccountDoesNotExist()
	{
		var accountId = 1;
		_mockAccountRepository.Setup(repo => repo.GetByIdAsync(accountId)).ReturnsAsync((Account?)null);

		Assert.ThrowsAsync<NotFoundException>(() => _accountDomain.GetByIdAsync(accountId));
	}


	[Test]
	public async Task AddAsync_ShouldAddNewAccount()
	{
		var account = new Account { UserId = 8, Balance = 100 };

		_mockAccountRepository.Setup(repo => repo.Add(account));
		_mockUnitOfWork.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

		_mockAccountRepository.Setup(repo => repo.GetByIdAsync(account.Id)).ReturnsAsync(account);

		var result = await _accountDomain.AddAsync(account);

		Assert.That(result, Is.Not.Null);
		Assert.That(result.UserId, Is.EqualTo(account.UserId));
		Assert.That(result.Balance, Is.EqualTo(account.Balance));

		_mockAccountRepository.Verify(repo => repo.Add(account), Times.Once);
		_mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
		_mockAccountRepository.Verify(repo => repo.GetByIdAsync(account.Id), Times.Once);
	}

	[Test]
	public void AddAsync_ShouldThrowException_WhenSavingFails()
	{
		var account = new Account { UserId = 8, Balance = 100 };

		_mockAccountRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Account?)null);

		_mockUnitOfWork.Setup(uow => uow.SaveAsync()).ThrowsAsync(new Exception("Save failed"));

		var ex = Assert.ThrowsAsync<Exception>(() => _accountDomain.AddAsync(account));
		Assert.That(ex.Message, Is.EqualTo("Save failed"));

		_mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
	}

	[Test]
	public async Task Update_ShouldUpdateAccountBalance()
	{
		var existingAccount = new Account { Id = 1, Balance = 100 };
		var updateRequest = new UpdateAccountRequest { Balance = 200 };

		_mockAccountRepository.Setup(repo => repo.GetByIdAsync(existingAccount.Id)).ReturnsAsync(existingAccount);
		_mockUnitOfWork.Setup(uow => uow.SaveAsync()).Returns(Task.CompletedTask);

		await _accountDomain.Update(existingAccount.Id, updateRequest);

		Assert.That(existingAccount.Balance, Is.EqualTo(200));

		_mockAccountRepository.Verify(repo => repo.GetByIdAsync(existingAccount.Id), Times.Once);
		_mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
	}

	[Test]
	public void Update_ShouldThrowNotFoundException_WhenAccountDoesNotExist()
	{
		var updateRequest = new UpdateAccountRequest { Balance = 200 };
		_mockAccountRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Account?)null);

		Assert.ThrowsAsync<NotFoundException>(() => _accountDomain.Update(1, updateRequest));

		_mockAccountRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
		_mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Never);
	}

	[Test]
	public async Task Delete_ShouldDeleteAccount()
	{
		var account = new Account { Id = 1, UserId = 8, Balance = 100 };

		_mockAccountRepository.Setup(repo => repo.GetByIdAsync(account.Id)).ReturnsAsync(account);
		_mockAccountRepository.Setup(repo => repo.DeleteAsync(account.Id));
		_mockUnitOfWork.Setup(uow => uow.SaveAsync()).Returns(Task.CompletedTask);

		await _accountDomain.DeleteAsync(account.Id);

		_mockAccountRepository.Verify(repo => repo.GetByIdAsync(account.Id), Times.Once);
		_mockAccountRepository.Verify(repo => repo.DeleteAsync(account.Id), Times.Once);
		_mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
	}

	[Test]
	public void Delete_ShouldThrowNotFoundException_WhenAccountDoesNotExist()
	{
		_mockAccountRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Account?)null);

		var ex = Assert.ThrowsAsync<NotFoundException>(() => _accountDomain.DeleteAsync(999));
		Assert.That(ex.Message, Is.EqualTo("Account Id: 999 not found"));

		_mockAccountRepository.Verify(repo => repo.GetByIdAsync(999), Times.Once);
		_mockAccountRepository.Verify(repo => repo.DeleteAsync(It.IsAny<int>()), Times.Never);
	}
}
