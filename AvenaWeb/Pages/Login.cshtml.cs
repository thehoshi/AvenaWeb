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

    public LoginModel(
        IUserRepository userRepository,
        IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    [BindProperty]
    public string Username { get; set; } = "";

    [BindProperty]
    public string Password { get; set; } = "";

    public async Task<IActionResult> OnPost()
    {
        var user = _userRepository.GetByUsername(Username.Trim());

        if (user == null)
        {
            ModelState.AddModelError("", "Invalid username or password.");
            return Page();
        }

        var result = _passwordHasher.VerifyHashedPassword(
            user,
            user.Password,
            Password);

        if (result == PasswordVerificationResult.Failed)
        {
            ModelState.AddModelError("", "Invalid username or password.");
            return Page();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

        var identity = new ClaimsIdentity(
            claims,
            "AvenaCookie");

        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
        "AvenaCookie",
        principal);

        return RedirectToPage("/Index");
    }
}