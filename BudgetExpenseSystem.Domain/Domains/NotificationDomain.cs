using BudgetExpenseSystem.Domain.Exceptions;
using BudgetExpenseSystem.Domain.Interfaces;
using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;

namespace BudgetExpenseSystem.Domain.Domains;

public class NotificationDomain : INotificationDomain
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly INotificationRepository _notificationRepository;

	public NotificationDomain(IUnitOfWork unitOfWork, INotificationRepository notificationRepository)
	{
		_unitOfWork = unitOfWork;
		_notificationRepository = notificationRepository;
	}

	public async Task<List<Notification>> GetAllAsync()
	{
		return await _notificationRepository.GetAllAsync();
	}


	public async Task<Notification> GetByIdAsync(int id)
	{
		var notification = await _notificationRepository.GetByIdAsync(id);
		if (notification == null) throw new NotFoundException($"Notification Id: {id} not found");

		return notification;
	}


	public async Task<Notification> AddAsync(Notification notification)
	{
		_notificationRepository.AddAsync(notification);

		await _unitOfWork.SaveAsync();
		return notification;
	}


	public async Task Update(int notificationId, UpdateNotificationRequset updateNotificationRequset)
	{
		var notification = await _notificationRepository.GetByIdAsync(notificationId);
		if (notification == null) throw new NotFoundException($"Notification Id: {notificationId} not found");

		notification.Name = updateNotificationRequset.Name;
		notification.Message = updateNotificationRequset.Message;

		_notificationRepository.Update(notification);
		await _unitOfWork.SaveAsync();
	}


	public async Task DeleteAsync(int id)
	{
		var notification = await _notificationRepository.GetByIdAsync(id);
		if (notification == null) throw new NotFoundException($"Category Id: {id} not found");

		await _notificationRepository.DeleteAsync(id);
		await _unitOfWork.SaveAsync();
	}
}