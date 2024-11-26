using BudgetExpenseSystem.Domain.CustomExceptions;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;
using MySqlConnector;

namespace BudgetExpenseSystem.Domain.Domains;

public class BudgetDomain
{
    private readonly IUnitOfWork _unitOfWork;

    public BudgetDomain(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<List<Budget>> RetrieveAllBudget()
    {
        return await _unitOfWork.GetRepository<Budget>().All();
    }
    
    
    public async Task<Budget?> FindBudgetById(int id)
    {
        try
        {
            return await _unitOfWork.GetRepository<Budget>().GetById(id);
        }
        catch (MySqlException ex)
        {
            throw new InvalidIdException($"Budget Id: {id} is invalid", ex);
        }
    }
    
    
    public async Task<Budget> HandleCreateBudget(Budget budget)
    {
        _unitOfWork.GetRepository<Budget>().Add(budget);
            
        await _unitOfWork.CompleteAsync();
        return budget;
    }
    
    
    
    public async void HandleUpdateBudget(Budget budget)
    {
        _unitOfWork.GetRepository<Budget>().Update(budget);
            
        await _unitOfWork.CompleteAsync();
    }
    
    
    public async Task<bool> HandleDeleteBudget(int id)
    {
        try
        {
            return await _unitOfWork.GetRepository<Budget>().Delete(id);
        }
        catch (Exception ex)
        {
            throw new InvalidIdException($"Budget Id: {id} is invalid", ex);
        }
    }
    
}