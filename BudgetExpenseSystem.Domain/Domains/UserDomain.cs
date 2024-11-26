using BudgetExpenseSystem.Domain.CustomExceptions;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;
using MySqlConnector;

namespace BudgetExpenseSystem.Domain.Domains;

public class UserDomain
{
    private readonly IUnitOfWork _unitOfWork;

    public UserDomain(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<User>> RetrieveAllUsers()
    {
            return await _unitOfWork.GetRepository<User>().All();
    }

    public async Task<User?> FindUserById(int id)
    {
        try
        {
            return await _unitOfWork.GetRepository<User>().GetById(id);
        }
        catch (MySqlException ex)
        {
            throw new InvalidIdException($"User Id: {id} is invalid", ex);
        }
    }

    public async Task<User> HandleCreateUser(User user)
    {
            _unitOfWork.GetRepository<User>().Add(user);

            await _unitOfWork.CompleteAsync();
            return user;
    }

    public async void HandleUpdateUser(User user)
    {
        _unitOfWork.GetRepository<User>().Update(user);

        await _unitOfWork.CompleteAsync();
    }

    public async Task<bool> HandleDeleteUser(int id)
    {
        try
        {
            return await _unitOfWork.GetRepository<User>().Delete(id);
        }
        catch (Exception ex)
        {
            throw new InvalidIdException($"User Id: {id} is invalid", ex);
        }
    }
    
}