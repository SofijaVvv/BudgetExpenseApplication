using BudgetExpenseSystem.Domain.CustomExceptions;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;
using MySqlConnector;

namespace BudgetExpenseSystem.Domain.Domains;

public class AccountDomain
{
    private readonly IUnitOfWork _unitOfWork;

    public AccountDomain(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Account>> RetrieveAllAccounts()
    {
        return await _unitOfWork.GetRepository<Account>().All();
    }
    
    
    public async Task<Account?> FindAccountById(int id)
    {
        try
        {
            return await _unitOfWork.GetRepository<Account>().GetById(id);
        }
        catch (MySqlException ex)
        {
            throw new InvalidIdException($"Account Id: {id} is invalid", ex);
        }
    }
    
    
    public async Task<Account> HandleCreateAccount(Account account)
    {
        _unitOfWork.GetRepository<Account>().Add(account);
            
        await _unitOfWork.CompleteAsync();
        return account;
    }
    
    
    
    public async void HandleUpdateAccount(Account account)
    {
        _unitOfWork.GetRepository<Account>().Update(account);
            
        await _unitOfWork.CompleteAsync();
    }
    
    
    public async Task<bool> HandleDeleteAccount(int id)
    {
        try
        {
            return await _unitOfWork.GetRepository<Account>().Delete(id);
        }
        catch (Exception ex)
        {
            throw new InvalidIdException($"Account Id: {id} is invalid", ex);
        }
    }
    
}