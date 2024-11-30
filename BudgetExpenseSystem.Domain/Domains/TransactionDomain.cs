using BudgetExpenseSystem.Domain.Interfaces;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;

namespace BudgetExpenseSystem.Domain.Domains;

public class TransactionDomain : IGenericDomain<Transaction>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<Transaction> _transactionRepository;

    public TransactionDomain(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _transactionRepository = _unitOfWork.GetRepository<Transaction>();
    }
    
    
    public async Task<List<Transaction>> GetAllAsync()
    {
        return await _transactionRepository.GetAllAsync();
    }

    public async Task<Transaction?> GetByIdAsync(int id)
    {
        return await _transactionRepository.GetByIdAsync(id);
    }

    public async Task<Transaction> AddAsync(Transaction transaction)
    {
        _transactionRepository.AddAsync(transaction);

        await _unitOfWork.SaveAsync();
        return transaction;
    }

    public async Task Update(Transaction transaction)
    {
        _transactionRepository.Update(transaction);

        await _unitOfWork.SaveAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _transactionRepository.DeleteAsync(id);
    }
}