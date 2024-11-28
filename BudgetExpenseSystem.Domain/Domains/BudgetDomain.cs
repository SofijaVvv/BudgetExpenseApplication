using BudgetExpenseSystem.Domain.Interfaces;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;

namespace BudgetExpenseSystem.Domain.Domains;

public class BudgetDomain : IGenericDomain<Budget>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<Budget> _genericRepository;

    public BudgetDomain(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _genericRepository = _unitOfWork.GetRepository<Budget>();
    }
    
    public async Task<List<Budget>> GetAllAsync()
    {
        return await _genericRepository.GetAllAsync();
    }
    
    
    public async Task<Budget?> GetByIdAsync(int id)
    {
        return await _genericRepository.GetByIdAsync(id);
    }
    
    
    public async Task<Budget> AddAsync(Budget budget)
    {
        _genericRepository.AddAsync(budget);
            
        await _unitOfWork.SaveAsync();
        return budget;
    }
    
    
    
    public async void Update(Budget budget)
    {
        _genericRepository.Update(budget);
            
        await _unitOfWork.SaveAsync();
    }
    
    
    public async Task<bool> DeleteAsync(int id)
    {
        return await _genericRepository.DeleteAsync(id);
    }
    
}