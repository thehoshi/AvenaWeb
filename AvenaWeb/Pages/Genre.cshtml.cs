using AvenaCore.Entities;
using AvenaCore.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AvenaWeb.Pages;

public class GenreModel : PageModel
{
    private readonly INewsRepository _newsRepository;
    private readonly IGenreRepository _genreRepository;

    public Genre? Genre { get; private set; }
    public List<News> News { get; private set; } = new();

    public GenreModel(
        INewsRepository newsRepository,
        IGenreRepository genreRepository)
    {
        _newsRepository = newsRepository;
        _genreRepository = genreRepository;
    }

    public IActionResult OnGet(int id)
    {
        Genre = _genreRepository.GetByID(id);

        if (Genre == null)
            return NotFound();

        News = _newsRepository.GetByGenreId(id);

        return Page();
    }
}