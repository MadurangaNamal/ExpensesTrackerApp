using ExpensesTracker.Data;
using ExpensesTracker.Models;

namespace ExpensesTracker.Services;

public class UserService : IUserService
{
    private readonly ExpensesTrackerDBContext _dbContext;

    public UserService(ExpensesTrackerDBContext dBContext)
    {
        _dbContext = dBContext ?? throw new ArgumentNullException(nameof(dBContext));
    }

    public async Task<UserAccount?> GetUserByIdAsync(int userId)
    {
        var user = await _dbContext.Users.FindAsync(userId);

        return user == null! ? throw new KeyNotFoundException($"User with ID {userId} not found.") : user;
    }

    public async Task AddUserOnLogin(UserAccount userAccount)
    {
        _dbContext.Add(userAccount);
        await _dbContext.SaveChangesAsync();
    }
}
