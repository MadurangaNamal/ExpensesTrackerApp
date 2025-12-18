using ExpensesTracker.Data;
using ExpensesTracker.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

var cookieExpireTime = builder.Configuration["AuthCookieExpiryMinutes"]
    ?? throw new InvalidOperationException("Cookie expiration time is not configured.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(long.Parse(cookieExpireTime));
    options.SlidingExpiration = true;
})
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"]
        ?? throw new InvalidOperationException("Google ClientId is not configured.");
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]
        ?? throw new InvalidOperationException("Google ClientSecret is not configured.");

    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.SaveTokens = true;

    //Request additional scopes (permissions) for email and profile information from google
    options.Scope.Add("email");
    options.Scope.Add("profile");

    // On successful authentication
    options.Events.OnCreatingTicket = context =>
    {
        var userEmail = context.Principal!.FindFirst(ClaimTypes.Email)?.Value;
        var userName = context.Principal.FindFirst(ClaimTypes.Name)?.Value;

        return Task.CompletedTask;
    };
});

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

var rawConnectionString = builder.Configuration.GetConnectionString("DefaultConnectionString")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnectionString' not found.");
var dbPassword = builder.Configuration["DB_PASSWORD"]
    ?? throw new InvalidOperationException("Database password not found in configuration.");
var connectionString = rawConnectionString!.Replace("{DB_PASSWORD}", dbPassword);

builder.Services.AddHttpLogging();
builder.Services.AddDbContext<ExpensesTrackerDBContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IExpensesService, ExpensesService>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

app.UseHttpLogging();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.MapStaticAssets();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Perform database migration at startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ExpensesTrackerDBContext>();
    await db.Database.MigrateAsync();
}

await app.RunAsync();
