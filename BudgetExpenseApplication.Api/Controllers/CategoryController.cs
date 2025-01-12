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
public class CategoryController : ControllerBase
{
	private readonly ICategoryDomain _categoryDomain;

	public CategoryController(ICategoryDomain categoryDomain)
	{
		_categoryDomain = categoryDomain;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CategoryResponse>))]
	public async Task<ActionResult<List<Category>>> GetAllCategories()
	{
		var categories = await _categoryDomain.GetAllAsync();

		var result = categories.ToResponse();
		return Ok(result);
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryResponse))]
	public async Task<ActionResult> GetCategoryById([FromRoute] int id)
	{
		var category = await _categoryDomain.GetByIdAsync(id);

		var result = category.ToResponse();
		return Ok(result);
	}

	[Authorize(Policy = "AdminOnly")]
	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CategoryResponse))]
	public async Task<ActionResult> AddCategory([FromBody] CategoryRequest categoryRequest)
	{
		var result = categoryRequest.ToCategory();
		await _categoryDomain.AddAsync(result);

		return CreatedAtAction(nameof(GetCategoryById), new { id = result.Id }, result.ToResponse());
	}

	[Authorize(Policy = "AdminOnly")]
	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<ActionResult> UpdateCategory(
		[FromRoute] int id,
		[FromBody] UpdateCategoryRequset updateCategoryRequset)
	{
		await _categoryDomain.Update(id, updateCategoryRequset);

		return NoContent();
	}

	[Authorize(Policy = "AdminOnly")]
	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<ActionResult> DeleteCategory([FromRoute] int id)
	{
		await _categoryDomain.DeleteAsync(id);
		return NoContent();
	}
}
