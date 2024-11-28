using BudgetExpenseSystem.Domain.Interfaces;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;

namespace BudgetExpenseSystem.Domain.Domains;

public class NotificationDomain : IGenericDomain<Notification>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<Notification> _genericRepository;

    public NotificationDomain(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _genericRepository = _unitOfWork.GetRepository<Notification>();
    }
    
    public async Task<List<Notification>> GetAllAsync()
    {
        return await _genericRepository.GetAllAsync();
    }
    
    
    public async Task<Notification?> GetByIdAsync(int id)
    {
        return await _genericRepository.GetByIdAsync(id);
    }
    
    
    public async Task<Notification> AddAsync(Notification notification)
    {
       _genericRepository.AddAsync(notification);
            
        await _unitOfWork.SaveAsync();
        return notification;
    }
    
    
    
    public async void Update(Notification notification)
    {
       _genericRepository.Update(notification);
            
        await _unitOfWork.SaveAsync();
    }
    
    
    public async Task<bool> DeleteAsync(int id)
    {
        return await _genericRepository.DeleteAsync(id);
    }
}