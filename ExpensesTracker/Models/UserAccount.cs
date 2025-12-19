using System.ComponentModel.DataAnnotations;

namespace ExpensesTracker.Models;

public class UserAccount
{
    [Key]
    public int UserId { get; set; }

    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [MaxLength(100, ErrorMessage = "Username should not exceed 100 characters")]
    public string UserName { get; set; } = null!;

    public ICollection<Expense> Expenses { get; set; } = [];
}
