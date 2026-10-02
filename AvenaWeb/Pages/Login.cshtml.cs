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

    public LoginModel(
        IUserRepository userRepository,
        IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    [BindProperty]
    public string Username { get; set; } = string.Empty;

    [BindProperty]
    public string Password { get; set; } = string.Empty;

    public IActionResult OnGet()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToPage("/Index");

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Username = Username.Trim();

        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            ModelState.AddModelError(string.Empty, "Username and password are required.");
            return Page();
        }

        User? user;

        try
        {
            user = _userRepository.GetByUsername(Username);
        }
        catch
        {
            ModelState.AddModelError(
                string.Empty,
                "Unable to connect to the database. Please try again later.");
            return Page();
        }

        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid username or password.");
            return Page();
        }

        var result = _passwordHasher.VerifyHashedPassword(
            user,
            user.Password,
            Password);

        if (result == PasswordVerificationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Invalid username or password.");
            return Page();
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.ID.ToString()),
            new(ClaimTypes.Name, user.Username)
        };

        var identity = new ClaimsIdentity(claims, AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            AuthenticationScheme,
            principal,
            new AuthenticationProperties
            {
                IsPersistent = true,
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(20)
            });

        return RedirectToPage("/Index");
    }
}
