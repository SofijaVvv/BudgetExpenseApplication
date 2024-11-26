using BudgetExpenseSystem.Domain.CustomExceptions;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;
using MySqlConnector;

namespace BudgetExpenseSystem.Domain.Domains;

public class RoleDomain
{
    private readonly IUnitOfWork _unitOfWork;

    public RoleDomain(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Role>> RetrieveAllRoles()
    {
            return await _unitOfWork.GetRepository<Role>().All();
    }

    
    public async Task<Role?> FindRoleById(int id)
    {
        try
        {
            return await _unitOfWork.GetRepository<Role>().GetById(id);
        }
        catch (MySqlException ex)
        {
            throw new InvalidIdException($"Role Id: {id} is invalid", ex);
        }
    }

    
    public async Task<Role> HandleCreateRole(Role role)
    {
            _unitOfWork.GetRepository<Role>().Add(role);
            
            await _unitOfWork.CompleteAsync();
            return role;
    }
    
    
    public async void HandleUpdateRole(Role role)
    {
            _unitOfWork.GetRepository<Role>().Update(role);
            
            await _unitOfWork.CompleteAsync();
    }
    
    
    public async Task<bool> HandleDeleteRole(int id)
    {
        try
        {
            return await _unitOfWork.GetRepository<Role>().Delete(id);
        }
        catch (Exception ex)
        {
            throw new InvalidIdException($"Role Id: {id} is invalid", ex);
        }
    }
    
    // public async Task<bool> HandleDeleteRole(int id)
    // {
    //     try
    //     {
    //         return await _unitOfWork.GetRepository<Role>().Delete(id);
    //     }
    //     catch (Exception ex)
    //     {
    //         throw new Exception($"An error occurred while deleting the role with ID {id}.", ex);
    //     }
    // }
    
}