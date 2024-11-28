using BudgetExpenseSystem.Domain.Interfaces;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;

namespace BudgetExpenseSystem.Domain.Domains;

public class BudgetTypeDomain : IGenericDomain<BudgetType>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<BudgetType> _genericRepository;

    public BudgetTypeDomain(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _genericRepository = _unitOfWork.GetRepository<BudgetType>();
    }
    
    public async Task<List<BudgetType>> GetAllAsync()
    {
        return await _genericRepository.GetAllAsync();
    }
    
    
    public async Task<BudgetType?> GetByIdAsync(int id)
    {
        return await _genericRepository.GetByIdAsync(id);
    }
    
    
    public async Task<BudgetType> AddAsync(BudgetType budgetType)
    {
        _genericRepository.AddAsync(budgetType);
            
        await _unitOfWork.SaveAsync();
        return budgetType;
    }
    
    
    
    public async void Update(BudgetType budgetType)
    {
        _genericRepository.Update(budgetType);
            
        await _unitOfWork.SaveAsync();
    }
    
    
    public async Task<bool> DeleteAsync(int id)
    {
        return await _genericRepository.DeleteAsync(id);
    }
}