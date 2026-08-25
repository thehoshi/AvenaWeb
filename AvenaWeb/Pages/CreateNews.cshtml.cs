using AvenaCore.Entities;
using AvenaCore.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
    public string? Image { get; set; }

    [BindProperty]
    public int GenreID { get; set; }

    public IActionResult OnGet()
    {
        if (!(User.Identity?.IsAuthenticated ?? false))
        {
            return Challenge();
        }

        Genres = _genreRepository.GetAll();

        return Page();
    }

    public IActionResult OnPost()
    {
        if (!(User.Identity?.IsAuthenticated ?? false))
        {
            return Challenge();
        }

        if (string.IsNullOrWhiteSpace(Title) ||
            string.IsNullOrWhiteSpace(Info))
        {
            ModelState.AddModelError("", "Title and text are required.");

            Genres = _genreRepository.GetAll();

            return Page();
        }

        var news = new News
        {
            Title = Title.Trim(),
            Info = Info.Trim(),
            Image = string.IsNullOrWhiteSpace(Image)
                ? null
                : Image.Trim(),
            Views = 0,
            CountOfLikes = 0,
            GenreID = GenreID,
            DateOfPost = DateTime.UtcNow,
            DeletedAt = null
        };

        int newId = _newsRepository.Create(news);

        return RedirectToPage("/News", new { id = newId });
    }
}