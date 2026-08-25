using AvenaCore.Entities;
using AvenaCore.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AvenaWeb.Pages;

public class NewsModel : PageModel
{
    private readonly INewsRepository _newsRepository;
    private readonly IGenreRepository _genreRepository;

    public News? News { get; private set; }
    public Genre? Genre { get; private set; }

    public NewsModel(
        INewsRepository newsRepository,
        IGenreRepository genreRepository)
    {
        _newsRepository = newsRepository;
        _genreRepository = genreRepository;
    }

    public IActionResult OnGet(int id)
    {
        News = _newsRepository.GetByID(id);

        if (News == null)
            return NotFound();

        Genre = _genreRepository.GetByID(News.GenreID);

        return Page();
    }
}