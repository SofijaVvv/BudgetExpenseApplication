using BudgetExpenseSystem.Domain.Domains;
using BudgetExpenseSystem.Domain.Interfaces;
using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Dto.Response;
using BudgetExpenseSystem.Model.Extentions;
using BudgetExpenseSystem.Model.Models;
using Microsoft.AspNetCore.Mvc;

namespace BudgetExpenseSystem.Api.Controllers;

[Route("api/[controller]s")]
[ApiController]
public class NotificationController : ControllerBase
{
	private readonly INotificationDomain _notificationDomain;

	public NotificationController(INotificationDomain notificationDomain)
	{
		_notificationDomain = notificationDomain;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<NotificationResponse>))]
	public async Task<ActionResult<List<Notification>>> GetAllNotifications()
	{
		var notifications = await _notificationDomain.GetAllAsync();

		var result = notifications.ToResponse();
		return Ok(result);
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NotificationResponse))]
	public async Task<ActionResult> GetNotificationById([FromRoute] int id)
	{
		var notification = await _notificationDomain.GetByIdAsync(id);

		var result = notification.ToResponse();
		return Ok(result);
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(NotificationResponse))]
	public async Task<ActionResult> AddNotification([FromBody] NotificationRequest notificationRequest)
	{
		var result = notificationRequest.ToNotification();
		await _notificationDomain.AddAsync(result);

		return CreatedAtAction(nameof(GetNotificationById), new { id = result.Id }, result);
	}

	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<ActionResult> UpdateNotification(
		[FromRoute] int id,
		[FromBody] UpdateNotificationRequset updateNotificationRequset)
	{
		await _notificationDomain.Update(id, updateNotificationRequset);

		return NoContent();
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<ActionResult> DeleteNotification([FromRoute] int id)
	{
		await _notificationDomain.DeleteAsync(id);
		return NoContent();
	}
}