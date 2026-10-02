using AvenaCore.Entities;
using AvenaCore.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace AvenaWeb.Pages;

public class LoginModel : PageModel
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    private const string AuthenticationScheme = "AvenaCookie";

    public LoginModel(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    [BindProperty] public string? Username { get; set; }
    [BindProperty] public string? Password { get; set; }
    public string? LoginError { get; private set; }

    public IActionResult OnGet()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToPage("/Index");
        return Page();
    }

    public async Task<IActionResult> OnPostLoginAsync()
    {
        Username = Username?.Trim();
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            LoginError = "Username and password are required.";
            return Page();
        }

        User? user;
        try { user = _userRepository.GetByUsername(Username); }
        catch { LoginError = "Unable to connect to the database. Please try again later."; return Page(); }

        if (user == null || _passwordHasher.VerifyHashedPassword(user, user.Password, Password) == PasswordVerificationResult.Failed)
        {
            LoginError = "Invalid username or password.";
            return Page();
        }

        await SignInUserAsync(user);
        return RedirectToPage("/Index");
    }

    private Task SignInUserAsync(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.ID.ToString()),
            new(ClaimTypes.Name, user.Username)
        };
        var identity = new ClaimsIdentity(claims, AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        return HttpContext.SignInAsync(AuthenticationScheme, principal, new AuthenticationProperties
        {
            IsPersistent = true,
            AllowRefresh = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(20)
        });
    }
}
