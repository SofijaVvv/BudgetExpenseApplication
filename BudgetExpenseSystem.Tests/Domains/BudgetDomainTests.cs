using BudgetExpenseSystem.Domain.Domains;
using BudgetExpenseSystem.Domain.Exceptions;
using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;
using Moq;

namespace BudgetExpenseSystem.Tests.Domains;

[TestFixture]
public class BudgetDomainTests
{
	private readonly Mock<IUnitOfWork> _mockUnitOfWork;
	private readonly Mock<IBudgetRepository> _mockBudgetRepository;
	private readonly Mock<ICategoryRepository> _mockCategoryRepository;
	private readonly Mock<IBudgetTypeRepository> _mockBudgetTypeRepository;
	private readonly BudgetDomain _budgetDomain;

	public BudgetDomainTests()
	{
		_mockUnitOfWork = new Mock<IUnitOfWork>();
		_mockBudgetRepository = new Mock<IBudgetRepository>();
		_mockCategoryRepository = new Mock<ICategoryRepository>();
		_mockBudgetTypeRepository = new Mock<IBudgetTypeRepository>();
		_budgetDomain = new BudgetDomain
		(
			_mockUnitOfWork.Object,
			_mockBudgetRepository.Object,
			_mockCategoryRepository.Object,
			_mockBudgetTypeRepository.Object
		);
	}

	[Test]
	public async Task GetAllAsync_ShouldReturnBudgets()
	{
		// Arrange
		var budgets = new List<Budget>
		{
			new() { Id = 1, UserId = 8, CategoryId = 1, BudgetTypeId = 2, Amount = 200 },
			new() { Id = 2, UserId = 2, CategoryId = 3, BudgetTypeId = 7, Amount = 300 }
		};
		_mockBudgetRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(budgets);

		// Act
		var result = await _budgetDomain.GetAllAsync();

		// Assert
		Assert.That(result.Count, Is.EqualTo(budgets.Count)); // ???
	}

	[Test]
	public async Task GetByIdAsync_ShouldReturnBudgetById()
	{
		// Arrange
		var budget = new Budget { Id = 1, UserId = 8, CategoryId = 1, BudgetTypeId = 2, Amount = 200 };

		_mockBudgetRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(budget);

		// Act
		var result = await _budgetDomain.GetByIdAsync(1);

		// Assert
		Assert.That(result, Is.Not.Null);
		Assert.That(result.Id, Is.EqualTo(budget.Id));
	}

	[Test]
	public void GetByIdAsync_ShouldThrowNotFoundException_WhenBudgetDoesNotExist()
	{
		// Arrange
		var budgetId = 1;
		_mockBudgetRepository.Setup(repo => repo.GetByIdAsync(budgetId)).ReturnsAsync((Budget?)null);

		// Assert
		Assert.ThrowsAsync<NotFoundException>(() => _budgetDomain.GetByIdAsync(budgetId));
	}

	[Test]
	public void UpdateBudgetFundsAsync_ShouldThrowBadRequestException_WhenCategoryDoesNotMatch()
	{
		// Arrange
		var budget = new Budget { Id = 1, CategoryId = 2, Amount = 500 };
		_mockBudgetRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(budget);

		// Assert
		Assert.ThrowsAsync<BadRequestException>(() => _budgetDomain.UpdateBudgetFundsAsync(1, 100, 1));
	}

	[Test]
	public void UpdateBudgetFundsAsync_ShouldThrowInsufficientFundsException_WhenFundsAreInsufficient()
	{
		// Arrange
		var budget = new Budget { Id = 1, CategoryId = 1, Amount = 50 };
		_mockBudgetRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(budget);

		// Assert
		Assert.ThrowsAsync<InsufficientFundsException>(async () =>
			await _budgetDomain.UpdateBudgetFundsAsync(1, -100, 1));
	}

	[Test]
	public async Task UpdateBudgetFundsAsync_ShouldUpdateBudget_WhenFundsAreSufficient()
	{
		// Arrange
		var budget = new Budget { Id = 1, CategoryId = 1, Amount = 500 };
		_mockBudgetRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(budget);

		//Act
		await _budgetDomain.UpdateBudgetFundsAsync(1, 100, 1);

		//Verify
		_mockBudgetRepository.Verify(repo => repo.Update(It.IsAny<Budget>()), Times.Once);
		_mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
	}

	[Test]
	public async Task AddAsync_ShouldAddNewBudget()
	{
		// Arrange
		var budget = new Budget { Id = 1, UserId = 8, CategoryId = 2, BudgetTypeId = 2, Amount = 200 };
		var category = new Category { Id = 2, Name = "Test Category" };
		var budgetType = new BudgetType { Id = 2, Name = "Test Budget Type" };

		_mockCategoryRepository.Setup(repo => repo.GetByIdAsync(budget.CategoryId)).ReturnsAsync(category);
		_mockBudgetTypeRepository.Setup(repo => repo.GetByIdAsync(budget.BudgetTypeId)).ReturnsAsync(budgetType);

		_mockBudgetRepository.Setup(repo => repo.AddAsync(budget));
		_mockUnitOfWork.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

		_mockBudgetRepository.Setup(repo => repo.GetByIdAsync(budget.Id)).ReturnsAsync(budget);

		// Act
		var result = await _budgetDomain.AddAsync(budget);

		// Assert
		Assert.That(result, Is.Not.Null);
		Assert.That(result.Id, Is.EqualTo(budget.Id));
		Assert.That(result.UserId, Is.EqualTo(budget.UserId));
		Assert.That(result.CategoryId, Is.EqualTo(budget.CategoryId));

		// Verify
		_mockBudgetRepository.Verify(repo => repo.AddAsync(budget), Times.Once);
		_mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
		_mockBudgetRepository.Verify(repo => repo.GetByIdAsync(budget.Id), Times.Once);
		_mockCategoryRepository.Verify(repo => repo.GetByIdAsync(budget.CategoryId), Times.Once);
		_mockBudgetTypeRepository.Verify(repo => repo.GetByIdAsync(budget.BudgetTypeId), Times.Once);
	}


	[Test]
	public void AddAsync_ShouldThrowNotFoundException_WhenCategoryDoesNotExist()
	{
		// Arrange
		var budget = new Budget { Id = 1, CategoryId = 1, Amount = 50 };
		_mockCategoryRepository.Setup(repo => repo.GetByIdAsync(budget.CategoryId)).ReturnsAsync((Category?)null);

		// Assert
		Assert.ThrowsAsync<NotFoundException>(() => _budgetDomain.AddAsync(budget));
	}

	[Test]
	public void AddAsync_ShouldThrowNotFoundException_WhenBudgetTypeDoesNotExist()
	{
		// Arrange
		var budget = new Budget { Id = 1, CategoryId = 1, BudgetTypeId = 2, Amount = 50 };
		_mockBudgetTypeRepository.Setup(repo => repo.GetByIdAsync(budget.CategoryId)).ReturnsAsync((BudgetType?)null);

		// Assert
		Assert.ThrowsAsync<NotFoundException>(() => _budgetDomain.AddAsync(budget));
	}

	[Test]
	public void Update_ShouldThrowNotFoundException_WhenBudgetDoesNotExist()
	{
		// Arrange
		var updateRequest = new UpdateBudgetRequest { CategoryId = 1, BudgetTypeId = 2, Amount = 200 };
		_mockBudgetRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Budget?)null);

		// Assert
		Assert.ThrowsAsync<NotFoundException>(() => _budgetDomain.Update(1, updateRequest));
	}

	[Test]
	public void Update_ShouldThrowNotFoundException_WhenCategoryDoesNotExist()
	{
		// Arrange
		var updateRequest = new UpdateBudgetRequest { CategoryId = 1, BudgetTypeId = 2, Amount = 200 };
		_mockCategoryRepository.Setup(repo => repo.GetByIdAsync(updateRequest.CategoryId))
			.ReturnsAsync((Category?)null);

		// Assert
		Assert.ThrowsAsync<NotFoundException>(() => _budgetDomain.Update(1, updateRequest));
	}

	[Test]
	public void Update_ShouldThrowNotFoundException_WhenBudgetTypeDoesNotExist()
	{
		// Arrange
		var updateRequest = new UpdateBudgetRequest { CategoryId = 1, BudgetTypeId = 2, Amount = 200 };
		_mockBudgetTypeRepository.Setup(repo => repo.GetByIdAsync(updateRequest.BudgetTypeId))
			.ReturnsAsync((BudgetType?)null);

		// Assert
		Assert.ThrowsAsync<NotFoundException>(() => _budgetDomain.Update(1, updateRequest));
	}

	[Test]
	public async Task Update_ShouldSuccessfullyUpdate_WhenValidDataProvided()
	{
		// Arrange
		var updateBudgetRequest = new UpdateBudgetRequest
		{
			CategoryId = 1,
			BudgetTypeId = 1,
			Amount = 100
		};

		var existingBudget = new Budget { Id = 1, CategoryId = 2, BudgetTypeId = 2, Amount = 50 };
		var category = new Category { Id = 1, Name = "Test Category" };
		var budgetType = new BudgetType { Id = 1, Name = "Test BudgetType" };

		_mockBudgetRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingBudget);
		_mockCategoryRepository.Setup(repo => repo.GetByIdAsync(updateBudgetRequest.CategoryId)).ReturnsAsync(category);
		_mockBudgetTypeRepository.Setup(repo => repo.GetByIdAsync(updateBudgetRequest.BudgetTypeId))
			.ReturnsAsync(budgetType);
		_mockUnitOfWork.Setup(uow => uow.SaveAsync()).Returns(Task.CompletedTask);

		await _budgetDomain.Update(1, updateBudgetRequest);

		// Assert
		Assert.That(existingBudget.CategoryId, Is.EqualTo(updateBudgetRequest.CategoryId));
		Assert.That(existingBudget.BudgetTypeId, Is.EqualTo(updateBudgetRequest.BudgetTypeId));
		Assert.That(existingBudget.Amount, Is.EqualTo(updateBudgetRequest.Amount));

		// Verify
		_mockBudgetRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
		_mockCategoryRepository.Verify(repo => repo.GetByIdAsync(updateBudgetRequest.CategoryId), Times.Once);
		_mockBudgetTypeRepository.Verify(repo => repo.GetByIdAsync(updateBudgetRequest.BudgetTypeId), Times.Once);
		_mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
	}

	[Test]
	public void Delete_ShouldThrowNotFoundException_WhenBudgetDoesNotExist()
	{
		// Arrange
		var budget = new Budget { Id = 2, CategoryId = 1, BudgetTypeId = 2, Amount = 200 };
		_mockBudgetRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Budget?)null);

		// Assert
		Assert.ThrowsAsync<NotFoundException>(() => _budgetDomain.DeleteAsync(budget.Id));
	}

	[Test]
	public async Task Delete_ShouldDeleteAccount()
	{
		// Arrange
		var budget = new Budget { Id = 2, CategoryId = 1, BudgetTypeId = 2, Amount = 200 };

		_mockBudgetRepository.Setup(repo => repo.GetByIdAsync(budget.Id)).ReturnsAsync(budget);
		_mockBudgetRepository.Setup(repo => repo.DeleteAsync(budget.Id));
		_mockUnitOfWork.Setup(uow => uow.SaveAsync()).Returns(Task.CompletedTask);

		//Act
		await _budgetDomain.DeleteAsync(budget.Id);

		//Verify
		_mockBudgetRepository.Verify(repo => repo.GetByIdAsync(budget.Id), Times.Once);
		_mockBudgetRepository.Verify(repo => repo.DeleteAsync(budget.Id), Times.Once);
		_mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
	}
}
