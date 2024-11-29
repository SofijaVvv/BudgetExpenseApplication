using BudgetExpenseSystem.Domain.Interfaces;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;

namespace BudgetExpenseSystem.Domain.Domains;

public class UserDomain : IGenericDomain<User>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<User> _genericRepository;

    public UserDomain(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _genericRepository = _unitOfWork.GetRepository<User>();
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _genericRepository.GetAllAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _genericRepository.GetByIdAsync(id);
    }

    public async Task<User> AddAsync(User user)
    {
            _genericRepository.AddAsync(user);

            await _unitOfWork.SaveAsync();
            return user;
    }

    public async Task Update(User user)
    {
        _genericRepository.Update(user);

        await _unitOfWork.SaveAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _genericRepository.DeleteAsync(id);
    }

}