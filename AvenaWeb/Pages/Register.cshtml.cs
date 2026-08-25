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
    public string Username { get; set; } = "";

    [BindProperty]
    public string Name { get; set; } = "";

    [BindProperty]
    public string Password { get; set; } = "";

    public IActionResult OnPost()
    {
        if (string.IsNullOrWhiteSpace(Username) ||
            string.IsNullOrWhiteSpace(Name) ||
            string.IsNullOrWhiteSpace(Password))
        {
            ModelState.AddModelError("", "All fields are required.");
            return Page();
        }

        if (_userRepository.GetByUsername(Username) != null)
        {
            ModelState.AddModelError("Username", "Username already exists.");
            return Page();
        }

        var user = new User
        {
            Username = Username.Trim(),
            Name = Name.Trim(),
            Password = "",
            AvatarImg = ""
        };

        user.Password = _passwordHasher.HashPassword(user, Password);

        _userRepository.Insert(user);

        return RedirectToPage("/Login");
    }
}