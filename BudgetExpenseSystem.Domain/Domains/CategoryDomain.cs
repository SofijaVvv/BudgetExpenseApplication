using BudgetExpenseSystem.Domain.CustomExceptions;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;
using MySqlConnector;

namespace BudgetExpenseSystem.Domain.Domains;

public class CategoryDomain
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryDomain(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<List<Category>> RetrieveAllCategory()
    {
        return await _unitOfWork.GetRepository<Category>().All();
    }
    
    
    public async Task<Category?> FindCategoryById(int id)
    {
        try
        {
            return await _unitOfWork.GetRepository<Category>().GetById(id);
        }
        catch (MySqlException ex)
        {
            throw new InvalidIdException($"Category Id: {id} is invalid", ex);
        }
    }
    
    
    public async Task<Category> HandleCreateCategory(Category category)
    {
        _unitOfWork.GetRepository<Category>().Add(category);
            
        await _unitOfWork.CompleteAsync();
        return category;
    }
    
    
    
    public async void HandleUpdateCategory(Category category)
    {
        _unitOfWork.GetRepository<Category>().Update(category);
            
        await _unitOfWork.CompleteAsync();
    }
    
    
    public async Task<bool> HandleDeleteCategory(int id)
    {
        try
        {
            return await _unitOfWork.GetRepository<Category>().Delete(id);
        }
        catch (Exception ex)
        {
            throw new InvalidIdException($"Category Id: {id} is invalid", ex);
        }
    }
}