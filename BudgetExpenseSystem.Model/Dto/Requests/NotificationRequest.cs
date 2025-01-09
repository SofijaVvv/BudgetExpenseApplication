namespace BudgetExpenseSystem.Model.Dto.Requests;

public class NotificationRequest
{
	public int UserId { get; set; }
	public string Name { get; set; }
	public string Message { get; set; }
}
