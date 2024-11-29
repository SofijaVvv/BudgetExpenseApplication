namespace BudgetExpenseSystem.Model.Dto.Requests;

public class UpdateNotificationRequset
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    public string Name { get; set; }
    
    public string Message { get; set; }
    
    public DateOnly CreatedDate { get; set; }
    public DateTime ReadAt { get; set; }
}