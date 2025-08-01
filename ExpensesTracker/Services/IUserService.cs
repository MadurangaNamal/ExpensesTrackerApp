﻿using ExpensesTracker.Models;

namespace ExpensesTracker.Services;

public interface IUserService
{
    Task<UserAccount?> GetUserByIdAsync(int userId);
    Task<UserAccount?> GetUserByEmailAsync(string email);
    Task<UserAccount?> GetCurrentUserAsync();
    Task AddUserOnLoginAsync(UserAccount userAccount);
    Task RemoveUserAsync(int userId);
}
