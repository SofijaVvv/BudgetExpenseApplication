using System.ComponentModel.DataAnnotations;

namespace BudgetExpenseSystem.Model.Models;

public class Notification
{
    [Key]
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Message { get; set; }
    
    public DateOnly CreatedDate { get; set; }
    public DateTime ReadAt { get; set; }
    
    public virtual User User { get; set; } 
    
}