using BudgetExpenseSystem.Domain.CustomExceptions;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;
using MySqlConnector;

namespace BudgetExpenseSystem.Domain.Domains;

public class BudgetTypeDomain
{
    private readonly IUnitOfWork _unitOfWork;

    public BudgetTypeDomain(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<List<BudgetType>> RetrieveAllBudgetTypes()
    {
        return await _unitOfWork.GetRepository<BudgetType>().All();
    }
    
    
    public async Task<BudgetType?> FindBudgetTypesById(int id)
    {
        try
        {
            return await _unitOfWork.GetRepository<BudgetType>().GetById(id);
        }
        catch (MySqlException ex)
        {
            throw new InvalidIdException($"BudgetType Id: {id} is invalid", ex);
        }
    }
    
    
    public async Task<BudgetType> HandleCreateBudgetTypes(BudgetType budgetType)
    {
        _unitOfWork.GetRepository<BudgetType>().Add(budgetType);
            
        await _unitOfWork.CompleteAsync();
        return budgetType;
    }
    
    
    
    public async void HandleUpdateBudgetTypes(BudgetType budgetType)
    {
        _unitOfWork.GetRepository<BudgetType>().Update(budgetType);
            
        await _unitOfWork.CompleteAsync();
    }
    
    
    public async Task<bool> HandleDeleteBudgetTypes(int id)
    {
        try
        {
            return await _unitOfWork.GetRepository<BudgetType>().Delete(id);
        }
        catch (Exception ex)
        {
            throw new InvalidIdException($"BudgetType Id: {id} is invalid", ex);
        }
    }
}