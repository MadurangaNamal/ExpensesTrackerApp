using ExpensesTracker.Models;
using ExpensesTracker.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpensesTracker.Controllers;

public class UserAccountsController : Controller
{
    private readonly IUserService _userService;

    public UserAccountsController(IUserService userService)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult GoogleLogin(bool rememberMe)
    {
        var authenticationProperties = new AuthenticationProperties
        {
            RedirectUri = Url.Action("GoogleResponse")
        };

        if (rememberMe)
        {
            authenticationProperties.IsPersistent = true;
            authenticationProperties.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7);
        }

        return Challenge(authenticationProperties, GoogleDefaults.AuthenticationScheme);
    }

    public async Task<IActionResult> GoogleResponse()
    {
        // Handle the response from Google after authentication
        var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        if (!authenticateResult.Succeeded)
            return RedirectToAction("Login");

        var claims = authenticateResult.Principal.Claims;
        var userEmail = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var userName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

        if ((!string.IsNullOrEmpty(userEmail)
            && !string.IsNullOrEmpty(userName)))
        {
            var user = await _userService.GetUserByEmailAsync(userEmail);

            // Create a new user account if not exist
            if (user == null)
            {
                await _userService.AddUserOnLoginAsync(new UserAccount
                {
                    Email = userEmail,
                    UserName = userName,
                });
            }
        }

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); // Sign out of both cookie authentication and Google
        return RedirectToAction("Index", "Home");
    }
}
