using AvenaCore.Entities;
using AvenaCore.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace AvenaWeb.Pages;

public class NewsModel : PageModel
{
    private readonly INewsRepository _newsRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly ICommentRepository _commentRepository;

    public News? News { get; private set; }
    public Genre? Genre { get; private set; }
    public List<Comment> Comments { get; private set; } = new();

    public NewsModel(
        INewsRepository newsRepository,
        IGenreRepository genreRepository,
        ICommentRepository commentRepository)
    {
        _newsRepository = newsRepository;
        _genreRepository = genreRepository;
        _commentRepository = commentRepository;
    }

    public IActionResult OnGet(int id)
    {
        News = _newsRepository.GetByID(id);

        if (News == null)
            return NotFound();

        Genre = _genreRepository.GetByID(News.GenreID);

        Comments = _commentRepository.GetByNewsID(id);

        return Page();
    }

    public IActionResult OnPost(int NewsId, string CommentText)
    {
        if (!User.Identity?.IsAuthenticated ?? true)
        {
            return Challenge();
        }

        if (string.IsNullOrWhiteSpace(CommentText))
        {
            return RedirectToPage("/News", new { id = NewsId });
        }

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdClaim, out int userId))
        {
            return Challenge();
        }

        var comment = new Comment
        {
            NewsID = NewsId,
            UserID = userId,
            Text = CommentText.Trim(),
            CountOfLikes = 0,
            DateOfPost = DateTime.UtcNow
        };

        _commentRepository.Insert(comment);

        return RedirectToPage("/News", new { id = NewsId });
    }
    public IActionResult OnPostLike(int id)
    {
        _newsRepository.AddLike(id);

        return RedirectToPage("/News", new { id });
    }
}