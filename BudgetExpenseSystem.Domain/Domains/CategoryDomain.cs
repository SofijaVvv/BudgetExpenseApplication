using BudgetExpenseSystem.Domain.Interfaces;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;

namespace BudgetExpenseSystem.Domain.Domains;

public class CategoryDomain 
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<Category> _genericRepository;

    public CategoryDomain(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _genericRepository = _unitOfWork.GetRepository<Category>();
    }
    
    public async Task<List<Category>> GetAllAsync()
    {
        return await _genericRepository.GetAllAsync();
    }
    
    
    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _genericRepository.GetByIdAsync(id);
    }
    
    
    public async Task<Category> AddAsync(Category category)
    {
        _genericRepository.AddAsync(category);
            
        await _unitOfWork.SaveAsync();
        return category;
    }
    
    
    
    public async Task Update(Category category)
    {
        _genericRepository.Update(category);
            
        await _unitOfWork.SaveAsync();
    }
    
    
    public async Task<bool> DeleteAsync(int id)
    {
        return await _genericRepository.DeleteAsync(id);
    }
}