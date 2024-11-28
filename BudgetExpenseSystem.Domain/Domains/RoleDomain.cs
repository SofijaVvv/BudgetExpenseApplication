using BudgetExpenseSystem.Domain.Interfaces;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;

namespace BudgetExpenseSystem.Domain.Domains;

public class RoleDomain : IGenericDomain<Role>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<Role> _genericRepository;

    public RoleDomain(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _genericRepository = _unitOfWork.GetRepository<Role>();
    }

    public async Task<List<Role>> GetAllAsync()
    {
            return await _genericRepository.GetAllAsync();
    }

    
    public async Task<Role?> GetByIdAsync(int id)
    {
            return await _genericRepository.GetByIdAsync(id);
    }

    
    public async Task<Role> AddAsync(Role role)
    {
            _genericRepository.AddAsync(role);
            
            await _unitOfWork.SaveAsync();
            return role;
    }
    
    
    public async void Update(Role role)
    {
            _genericRepository.Update(role);
            
            await _unitOfWork.SaveAsync();
    }
    
    
    public async Task<bool> DeleteAsync(int id)
    {
            return await _genericRepository.DeleteAsync(id);
    }
}