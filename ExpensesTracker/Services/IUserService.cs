using ExpensesTracker.Models;

namespace ExpensesTracker.Services;

public interface IUserService
{
    Task<UserAccount?> GetUserByIdAsync(int userId);
    Task AddUserOnLogin(UserAccount userAccount);
}
