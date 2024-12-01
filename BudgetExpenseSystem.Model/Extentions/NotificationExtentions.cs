using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Dto.Response;
using BudgetExpenseSystem.Model.Models;

namespace BudgetExpenseSystem.Model.Extentions;

public static class NotificationExtentions
{
	public static Notification ToNotification(this NotificationRequest request)
	{
		return new Notification
		{
			UserId = request.UserId,
			Name = request.Name,
			Message = request.Message,
			CreatedDate = request.CreatedDate,
			ReadAt = request.ReadAt
		};
	}

	public static NotificationResponse ToResponse(this Notification response)
	{
		return new NotificationResponse
		{
			Id = response.Id,
			UserId = response.UserId,
			Name = response.Name,
			Message = response.Message,
			CreatedDate = response.CreatedDate,
			ReadAt = response.ReadAt
		};
	}

	public static List<NotificationResponse> ToResponse(this List<Notification> response)
	{
		return response.Select(notification => notification.ToResponse()).ToList();
	}
}