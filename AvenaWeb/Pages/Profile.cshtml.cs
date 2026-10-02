using AvenaCore.Entities;
using AvenaCore.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using AvenaWeb.Services;

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

    public async Task<IActionResult> OnPostAsync(string name, IFormFile? avatarFile)
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

        string avatarPath = CurrentUser.AvatarImg;

        if (avatarFile != null && avatarFile.Length > 0)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

            var extension = Path.GetExtension(avatarFile.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError(string.Empty, "Only JPG, JPEG, PNG and WebP images are allowed.");
                return Page();
            }

            var fileName = $"{Guid.NewGuid()}{extension}";

            var contentType = extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".webp" => "image/webp",
                _ => "application/octet-stream"
            };

            using (var stream = avatarFile.OpenReadStream())
            {
                var imageStorageService = HttpContext.RequestServices.GetRequiredService<IImageStorageService>();
                avatarPath = await imageStorageService.UploadAsync(stream, fileName, contentType);
            }
        }

        _userRepository.UpdateProfile(userId, name.Trim(), avatarPath);

        return RedirectToPage();
    }
}