using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpensesTracker.Controllers;

public class UserAccountsController : Controller
{
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult GoogleLogin()
    {
        var authenticationProperties = new AuthenticationProperties
        {
            RedirectUri = Url.Action("GoogleResponse")
        };

        return Challenge(authenticationProperties, GoogleDefaults.AuthenticationScheme);
    }

    public async Task<IActionResult> GoogleResponse()
    {
        //Handle the response from Google after authentication
        var authenticateResult = await HttpContext.AuthenticateAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        if (!authenticateResult.Succeeded)
            return RedirectToAction("Login");

        var claims = authenticateResult.Principal.Claims;
        var userEmail = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var userName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        //Sign out of both cookie authentication and Google
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}
