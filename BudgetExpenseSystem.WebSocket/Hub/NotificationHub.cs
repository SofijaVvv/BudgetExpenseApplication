using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BudgetExpenseSystem.WebSocket.Hub;

[Authorize]
public class NotificationHub : Microsoft.AspNetCore.SignalR.Hub
{
	public override Task OnConnectedAsync()
	{
		var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		var role = Context.User?.FindFirst(ClaimTypes.Role)?.Value;

		if (userId == null) throw new Exception("User identifier not found.");
		Console.WriteLine($"User connected with ID: {userId}, Role: {role}");
		return base.OnConnectedAsync();
	}

	public async Task SendTransactionNotification(string message)
	{
		var userId = Context.UserIdentifier;
		if (userId != null)
		{
			await Clients.User(userId).SendAsync("ReceiveTransactionNotification", message);
		}
		else
		{
			Console.WriteLine("User identifier is not available.");
		}
	}
}
