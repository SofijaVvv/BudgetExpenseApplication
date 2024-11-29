using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;

namespace BudgetExpenseSystem.Domain.Domains;

public class AccountDomain 
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<Account> _accountRepository;

    public AccountDomain(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _accountRepository = _unitOfWork.GetRepository<Account>();
    }

    public async Task<List<Account>> GetAllAsync()
    {
        return await _accountRepository.GetAllAsync();
    }
    
    
    public async Task<Account?> GetByIdAsync(int id)
    {
        return await _accountRepository.GetByIdAsync(id);
    }
    
    
    public async Task<Account> AddAsync(Account account)
    {
        _accountRepository.AddAsync(account);
            
        await _unitOfWork.SaveAsync();
        return account;
    }
    
    
    
    public async Task Update(int accountId, UpdateAccountRequest updateAccountRequest)
    {
        Account? account = await _accountRepository.GetByIdAsync(accountId);
        if (account is null)
        {
            throw new Exception("Account not found"); 
            // mozes napraviti custom NotFoundException pa da ga catch u controller i vratis 404
        }

        account.Balance = updateAccountRequest.Balance;
        //_accountRepository.Update(account);
            
        await _unitOfWork.SaveAsync();
    }
    
    
    public async Task<bool> DeleteAsync(int id)
    {
        return await _accountRepository.DeleteAsync(id);
    }
    
}