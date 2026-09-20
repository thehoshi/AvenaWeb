using AvenaCore.Entities;
using AvenaCore.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using AvenaWeb.Services;

namespace AvenaWeb.Pages;

public class CreateNewsModel : PageModel
{
    private readonly INewsRepository _newsRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IImageStorageService _imageStorageService;

    public CreateNewsModel(
        INewsRepository newsRepository,
        IGenreRepository genreRepository,
        IImageStorageService imageStorageService)
    {
        _newsRepository = newsRepository;
        _genreRepository = genreRepository;
        _imageStorageService = imageStorageService;
    }

    public List<Genre> Genres { get; private set; } = new();

    [BindProperty]
    public string Title { get; set; } = "";

    [BindProperty]
    public string Info { get; set; } = "";

    [BindProperty]
    public IFormFile? ImageFile { get; set; }

    [BindProperty]
    public string GenreName { get; set; } = "";

    public IActionResult OnGet()
    {
        if (!(User.Identity?.IsAuthenticated ?? false))
        {
            return Challenge();
        }

        Genres = _genreRepository.GetAll();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!(User.Identity?.IsAuthenticated ?? false))
        {
            return Challenge();
        }

        if (string.IsNullOrWhiteSpace(Title) ||
            string.IsNullOrWhiteSpace(Info) ||
            string.IsNullOrWhiteSpace(GenreName))
        {
            ModelState.AddModelError("", "Title, text, and genre are required.");

            Genres = _genreRepository.GetAll();

            return Page();
        }

        int genreId = _genreRepository.GetOrCreate(GenreName.Trim());

        string? imagePath = null;

        if (ImageFile != null && ImageFile.Length > 0)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

            var extension = Path.GetExtension(ImageFile.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("ImageFile", "Only JPG, JPEG, PNG and WebP images are allowed.");

                Genres = _genreRepository.GetAll();

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

            using (var stream = ImageFile.OpenReadStream())
            {
                imagePath = await _imageStorageService.UploadAsync(stream, fileName, contentType);
            }
        }

        var news = new News
        {
            Title = Title.Trim(),
            Info = Info.Trim(),
            Image = imagePath,
            Views = 0,
            CountOfLikes = 0,
            GenreID = genreId,
            DateOfPost = DateTime.UtcNow,
            DeletedAt = null
        };

        int newId = _newsRepository.Create(news);

        return RedirectToPage("/News", new { id = newId });
    }
}