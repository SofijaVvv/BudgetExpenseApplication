namespace BudgetExpenseSystem.Model.Dto;

public record ErrorModel
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public string? Details { get; set; } 
}
