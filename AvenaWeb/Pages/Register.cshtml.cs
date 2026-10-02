using AvenaCore.Entities;
using AvenaCore.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace AvenaWeb.Pages;

public class RegisterModel : PageModel
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    private const string AuthenticationScheme = "AvenaCookie";

    public RegisterModel(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    [BindProperty] public string? Username { get; set; }
    [BindProperty] public string? Name { get; set; }
    [BindProperty] public string? Password { get; set; }
    public string? ErrorMessage { get; private set; }

    public IActionResult OnGet()
    {
        if (User.Identity?.IsAuthenticated == true) return RedirectToPage("/Index");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Username = Username?.Trim();
        Name = Name?.Trim();

        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "All registration fields are required.";
            return Page();
        }

        if (Username.Length < 3) { ErrorMessage = "Username must contain at least 3 characters."; return Page(); }
        if (Password.Length < 6) { ErrorMessage = "Password must contain at least 6 characters."; return Page(); }

        try
        {
            if (_userRepository.GetByUsername(Username) != null)
            {
                ErrorMessage = "Username already exists.";
                return Page();
            }

            var user = new User
            {
                Username = Username,
                Name = Name,
                AvatarImg = string.Empty,
                Password = string.Empty
            };
            user.Password = _passwordHasher.HashPassword(user, Password);
            _userRepository.Insert(user);
            var createdUser = _userRepository.GetByUsername(Username) ?? user;
            await SignInUserAsync(createdUser);
            return RedirectToPage("/Index");
        }
        catch
        {
            ErrorMessage = "Unable to create the account. Please try again later.";
            return Page();
        }
    }

    private Task SignInUserAsync(User user)
    {
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        }, AuthenticationScheme);
        return HttpContext.SignInAsync(AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties
        { IsPersistent = true, AllowRefresh = true, ExpiresUtc = DateTimeOffset.UtcNow.AddDays(20) });
    }
}
