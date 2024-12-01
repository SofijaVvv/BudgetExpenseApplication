using BudgetExpenseSystem.Domain.Exceptions;
using BudgetExpenseSystem.Domain.Interfaces;
using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;

namespace BudgetExpenseSystem.Domain.Domains;

public class CategoryDomain
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IGenericRepository<Category> _categoryRepository;

	public CategoryDomain(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
		_categoryRepository = _unitOfWork.GetRepository<Category>();
	}

	public async Task<List<Category>> GetAllAsync()
	{
		return await _categoryRepository.GetAllAsync();
	}


	public async Task<Category> GetByIdAsync(int id)
	{
		var category = await _categoryRepository.GetByIdAsync(id);
		if (category == null) throw new NotFoundException($"Category Id: {id} not found");

		return category;
	}


	public async Task<Category> AddAsync(Category category)
	{
		_categoryRepository.AddAsync(category);

		await _unitOfWork.SaveAsync();
		return category;
	}


	public async Task Update(int categoryId, UpdateCategoryRequset updateCategoryRequset)
	{
		var category = await _categoryRepository.GetByIdAsync(categoryId);
		if (category == null) throw new NotFoundException($"Category Id: {categoryId} not found");

		category.Name = updateCategoryRequset.Name;
		category.Description = updateCategoryRequset.Description;

		_categoryRepository.Update(category);
		await _unitOfWork.SaveAsync();
	}


	public async Task DeleteAsync(int id)
	{
		var category = await _categoryRepository.GetByIdAsync(id);
		if (category == null) throw new NotFoundException($"Category Id: {id} not found");

		await _categoryRepository.DeleteAsync(id);
		await _unitOfWork.SaveAsync();
	}
}
