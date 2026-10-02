using AvenaCore.Entities;
using AvenaCore.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AvenaWeb.Pages;

public class RegisterModel : PageModel
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;

    public RegisterModel(
        IUserRepository userRepository,
        IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    [BindProperty]
    public string Username { get; set; } = string.Empty;

    [BindProperty]
    public string Name { get; set; } = string.Empty;

    [BindProperty]
    public string Password { get; set; } = string.Empty;

    public IActionResult OnPost()
    {
        Username = Username.Trim();
        Name = Name.Trim();

        if (string.IsNullOrWhiteSpace(Username) ||
            string.IsNullOrWhiteSpace(Name) ||
            string.IsNullOrWhiteSpace(Password))
        {
            ModelState.AddModelError(string.Empty, "All fields are required.");
            return Page();
        }

        if (Username.Length < 3)
        {
            ModelState.AddModelError("Username", "Username must contain at least 3 characters.");
            return Page();
        }

        if (Password.Length < 6)
        {
            ModelState.AddModelError("Password", "Password must contain at least 6 characters.");
            return Page();
        }

        try
        {
            if (_userRepository.GetByUsername(Username) != null)
            {
                ModelState.AddModelError("Username", "Username already exists.");
                return Page();
            }

            var user = new User
            {
                Username = Username,
                Name = Name,
                AvatarImg = string.Empty
            };

            user.Password = _passwordHasher.HashPassword(user, Password);
            _userRepository.Insert(user);
        }
        catch
        {
            ModelState.AddModelError(
                string.Empty,
                "Unable to create the account. Please try again later.");
            return Page();
        }

        return RedirectToPage("/Login");
    }
}
