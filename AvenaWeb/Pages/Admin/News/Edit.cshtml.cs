using AvenaCore.Entities;
using AvenaCore.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AvenaWeb.Pages.Admin.News;

public class EditModel : PageModel
{
    private readonly INewsRepository _newsRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IAuthorRepository _authorRepository;

    public AvenaCore.Entities.News? Article { get; private set; }
    public List<Genre> Genres { get; private set; } = new();
    public List<Author> Authors { get; private set; } = new();
    public int? Id { get; private set; }
    public string Status => Article?.Status ?? "new";

    public EditModel(
        INewsRepository newsRepository,
        IGenreRepository genreRepository,
        IAuthorRepository authorRepository)
    {
        _newsRepository = newsRepository;
        _genreRepository = genreRepository;
        _authorRepository = authorRepository;
    }

    public IActionResult OnGet(int? id)
    {
        Genres = _genreRepository.GetAll();
        Authors = _authorRepository.GetAll();
        Id = id;

        if (id.HasValue)
        {
            Article = _newsRepository.GetByIDForAdmin(id.Value);
            if (Article == null)
                return NotFound();
        }

        return Page();
    }
}
