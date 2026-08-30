using AvenaCore.Entities;
using AvenaCore.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;

namespace AvenaWeb.Pages;

public class CreateNewsModel : PageModel
{
    private readonly INewsRepository _newsRepository;
    private readonly IGenreRepository _genreRepository;

    public CreateNewsModel(
        INewsRepository newsRepository,
        IGenreRepository genreRepository)
    {
        _newsRepository = newsRepository;
        _genreRepository = genreRepository;
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

            var uploadsFolder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "images",
                "news");

            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{extension}";

            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await ImageFile.CopyToAsync(stream);
            }

            imagePath = $"/images/news/{fileName}";
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