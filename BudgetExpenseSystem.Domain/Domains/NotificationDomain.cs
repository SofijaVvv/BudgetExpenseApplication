using BudgetExpenseSystem.Domain.CustomExceptions;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;
using MySqlConnector;

namespace BudgetExpenseSystem.Domain.Domains;

public class NotificationDomain
{
    private readonly IUnitOfWork _unitOfWork;

    public NotificationDomain(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<List<Notification>> RetrieveAllNotification()
    {
        return await _unitOfWork.GetRepository<Notification>().All();
    }
    
    
    public async Task<Notification?> FindNotificationById(int id)
    {
        try
        {
            return await _unitOfWork.GetRepository<Notification>().GetById(id);
        }
        catch (MySqlException ex)
        {
            throw new InvalidIdException($"Notification Id: {id} is invalid", ex);
        }
    }
    
    
    public async Task<Notification> HandleCreateNotification(Notification notification)
    {
        _unitOfWork.GetRepository<Notification>().Add(notification);
            
        await _unitOfWork.CompleteAsync();
        return notification;
    }
    
    
    
    public async void HandleUpdateNotification(Notification notification)
    {
        _unitOfWork.GetRepository<Notification>().Update(notification);
            
        await _unitOfWork.CompleteAsync();
    }
    
    
    public async Task<bool> HandleDeleteNotification(int id)
    {
        try
        {
            return await _unitOfWork.GetRepository<Notification>().Delete(id);
        }
        catch (Exception ex)
        {
            throw new InvalidIdException($"Notification Id: {id} is invalid", ex);
        }
    }
}