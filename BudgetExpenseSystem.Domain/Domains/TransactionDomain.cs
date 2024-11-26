using BudgetExpenseSystem.Domain.CustomExceptions;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;
using MySqlConnector;

namespace BudgetExpenseSystem.Domain.Domains;

public class TransactionDomain
{
    private readonly IUnitOfWork _unitOfWork;

    public TransactionDomain(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    
    public async Task<List<Transaction>> RetrieveAllTransactions()
    {
        return await _unitOfWork.GetRepository<Transaction>().All();
    }

    public async Task<Transaction?> FindTransactionById(int id)
    {
        try
        {
            return await _unitOfWork.GetRepository<Transaction>().GetById(id);
        }
        catch (MySqlException ex)
        {
            throw new InvalidIdException($"Transaction Id: {id} is invalid", ex);
        }
    }

    public async Task<Transaction> HandleCreateTransaction(Transaction transaction)
    {
        _unitOfWork.GetRepository<Transaction>().Add(transaction);

        await _unitOfWork.CompleteAsync();
        return transaction;
    }

    public async void HandleUpdateTransaction(Transaction transaction)
    {
        _unitOfWork.GetRepository<Transaction>().Update(transaction);

        await _unitOfWork.CompleteAsync();
    }

    public async Task<bool> HandleDeleteTransaction(int id)
    {
        try
        {
            return await _unitOfWork.GetRepository<Transaction>().Delete(id);
        }
        catch (Exception ex)
        {
            throw new InvalidIdException($"Transaction Id: {id} is invalid", ex);
        }
    }
}