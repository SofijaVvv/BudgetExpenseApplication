using BudgetExpenseSystem.Domain.Interfaces;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;

namespace BudgetExpenseSystem.Domain.Domains;

public class BudgetTypeDomain 
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<BudgetType> _budgetTypeRepository;

    public BudgetTypeDomain(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _budgetTypeRepository = _unitOfWork.GetRepository<BudgetType>();
    }
    
    public async Task<List<BudgetType>> GetAllAsync()
    {
        return await _budgetTypeRepository.GetAllAsync();
    }
    
    
    public async Task<BudgetType?> GetByIdAsync(int id)
    {
        return await _budgetTypeRepository.GetByIdAsync(id);
    }
    
    
    public async Task<BudgetType> AddAsync(BudgetType budgetType)
    {
        _budgetTypeRepository.AddAsync(budgetType);
            
        await _unitOfWork.SaveAsync();
        return budgetType;
    }
    
    
    
    public async Task Update(BudgetType budgetType)
    {
        _budgetTypeRepository.Update(budgetType);
            
        await _unitOfWork.SaveAsync();
    }
    
    
    public async Task<bool> DeleteAsync(int id)
    {
        return await _budgetTypeRepository.DeleteAsync(id);
    }
}