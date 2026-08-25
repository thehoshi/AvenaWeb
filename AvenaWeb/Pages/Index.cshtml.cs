using AvenaCore.Entities;
using AvenaCore.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AvenaWeb.Pages;

public class IndexModel : PageModel
{
    private readonly INewsRepository _newsRepository;
    private readonly IGenreRepository _genreRepository;

    public List<Genre> Genres { get; private set; } = new();
    public List<News> News { get; private set; } = new();

    public IndexModel(
    INewsRepository newsRepository,
    IGenreRepository genreRepository)
    {
        _newsRepository = newsRepository;
        _genreRepository = genreRepository;
    }
    public void OnGet()
    {
        News = _newsRepository.GetAll();
        Genres = _genreRepository.GetAll();
    }

}