using BudgetExpenseSystem.Domain.Interfaces;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;

namespace BudgetExpenseSystem.Domain.Domains;

public class NotificationDomain 
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<Notification> _notificationRepository;

    public NotificationDomain(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _notificationRepository = _unitOfWork.GetRepository<Notification>();
    }
    
    public async Task<List<Notification>> GetAllAsync()
    {
        return await _notificationRepository.GetAllAsync();
    }
    
    
    public async Task<Notification?> GetByIdAsync(int id)
    {
        return await _notificationRepository.GetByIdAsync(id);
    }
    
    
    public async Task<Notification> AddAsync(Notification notification)
    {
       _notificationRepository.AddAsync(notification);
            
        await _unitOfWork.SaveAsync();
        return notification;
    }
    
    
    
    public async Task Update(Notification notification)
    {
       _notificationRepository.Update(notification);
            
        await _unitOfWork.SaveAsync();
    }
    
    
    public async Task<bool> DeleteAsync(int id)
    {
        return await _notificationRepository.DeleteAsync(id);
    }
}