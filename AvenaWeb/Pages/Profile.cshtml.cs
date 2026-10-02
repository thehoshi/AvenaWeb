using AvenaCore.Entities;
using AvenaCore.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace AvenaWeb.Pages;

public class ProfileModel : PageModel
{
    private readonly IUserRepository _userRepository;

    public User? CurrentUser { get; private set; }

    public ProfileModel(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public IActionResult OnGet()
    {
        if (!(User.Identity?.IsAuthenticated ?? false))
        {
            return Challenge();
        }

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdClaim, out int userId))
        {
            return Challenge();
        }

        CurrentUser = _userRepository.GetByID(userId);

        if (CurrentUser == null)
        {
            return NotFound();
        }

        return Page();
    }

    public IActionResult OnPost(string name)
    {
        if (!(User.Identity?.IsAuthenticated ?? false))
        {
            return Challenge();
        }

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdClaim, out int userId))
        {
            return Challenge();
        }

        CurrentUser = _userRepository.GetByID(userId);

        if (CurrentUser == null)
        {
            return NotFound();
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            ModelState.AddModelError(string.Empty, "Name can't be empty.");
            return Page();
        }

        _userRepository.UpdateProfile(userId, name.Trim(), CurrentUser.AvatarImg);


        return RedirectToPage();
    }
}