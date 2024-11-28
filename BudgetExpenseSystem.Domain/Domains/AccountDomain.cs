using BudgetExpenseSystem.Domain.Interfaces;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;

namespace BudgetExpenseSystem.Domain.Domains;

public class AccountDomain : IGenericDomain<Account>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<Account> _genericRepository;

    public AccountDomain(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _genericRepository = _unitOfWork.GetRepository<Account>();
    }

    public async Task<List<Account>> GetAllAsync()
    {
        return await _genericRepository.GetAllAsync();
    }
    
    
    public async Task<Account?> GetByIdAsync(int id)
    {
        return await _genericRepository.GetByIdAsync(id);
    }
    
    
    public async Task<Account> AddAsync(Account account)
    {
        _genericRepository.AddAsync(account);
            
        await _unitOfWork.SaveAsync();
        return account;
    }
    
    
    
    public async void Update(Account account)
    {
        _genericRepository.Update(account);
            
        await _unitOfWork.SaveAsync();
    }
    
    
    public async Task<bool> DeleteAsync(int id)
    {
        return await _genericRepository.DeleteAsync(id);
    }
    
}