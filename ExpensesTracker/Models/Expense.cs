using System.ComponentModel.DataAnnotations;

namespace ExpensesTracker.Models;

public class Expense
{
    public int Id { get; set; }

    [Required]
    [MaxLength(1000, ErrorMessage = "Description should not exceed 1000 characters")]
    public string Description { get; set; } = null!;

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount needs to be higher than 0")]
    public double Amount { get; set; }

    [Required]
    public Category? Category { get; set; } = null!;

    public DateTime Date { get; set; } = DateTime.Now;
}
