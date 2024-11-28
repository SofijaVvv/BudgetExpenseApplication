using System.ComponentModel.DataAnnotations;

namespace BudgetExpenseSystem.Model.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Email { get; set; }
    
    public string PasswordHash { get; set; }
    
    public string PasswordSalt { get; set; }
    
    public int RoleId { get; set; }
    
    public virtual Role Role { get; set; } 
    
}