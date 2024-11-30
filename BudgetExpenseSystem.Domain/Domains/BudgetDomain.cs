using BudgetExpenseSystem.Domain.Interfaces;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;

namespace BudgetExpenseSystem.Domain.Domains;

public class BudgetDomain
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<Budget> _budgetRepository;

    public BudgetDomain(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _budgetRepository = _unitOfWork.GetRepository<Budget>();
    }
    
    public async Task<List<Budget>> GetAllAsync()
    {
        return await _budgetRepository.GetAllAsync();
    }
    
    
    public async Task<Budget?> GetByIdAsync(int id)
    {
        return await _budgetRepository.GetByIdAsync(id);
    }
    
    
    public async Task<Budget> AddAsync(Budget budget)
    {
        _budgetRepository.AddAsync(budget);
            
        await _unitOfWork.SaveAsync();
        return budget;
    }
    
    
    
    public async Task Update(Budget budget)
    {
        _budgetRepository.Update(budget);
            
        await _unitOfWork.SaveAsync();
    }
    
    
    public async Task<bool> DeleteAsync(int id)
    {
        return await _budgetRepository.DeleteAsync(id);
    }
    
}