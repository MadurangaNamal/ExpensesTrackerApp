using ExpensesTracker.Data;
using ExpensesTracker.Models;
using Microsoft.EntityFrameworkCore;

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
        var user = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserId == userId);
        return user;
    }

    public async Task<UserAccount?> GetUserByEmailAsync(string email)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
        return user;
    }

    public async Task AddUserOnLoginAsync(UserAccount userAccount)
    {
        _dbContext.Add(userAccount);
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveUserAsync(int userId)
    {
        var user = await _dbContext.Users.FindAsync(userId);

        if (user == null)
            throw new KeyNotFoundException($"User with ID {userId} not found.");

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }
}
