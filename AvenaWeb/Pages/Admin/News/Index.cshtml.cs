using AvenaCore.Entities;
using AvenaCore.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AvenaWeb.Pages.Admin.News;

public class IndexModel : PageModel
{
    private readonly INewsRepository _newsRepository;

    public List<AvenaCore.Entities.News> Articles { get; private set; } = new();

    public IndexModel(INewsRepository newsRepository)
    {
        _newsRepository = newsRepository;
    }

    public void OnGet()
    {
        Articles = _newsRepository.GetAllForAdmin();
    }
}
