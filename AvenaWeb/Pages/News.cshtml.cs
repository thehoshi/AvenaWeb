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
    private readonly IUserRepository _userRepository;

    public News? News { get; private set; }
    public Genre? Genre { get; private set; }
    public List<CommentWithUser> Comments { get; private set; } = new();

    public NewsModel(
        INewsRepository newsRepository,
        IGenreRepository genreRepository,
        ICommentRepository commentRepository,
        IUserRepository userRepository)
    {
        _newsRepository = newsRepository;
        _genreRepository = genreRepository;
        _commentRepository = commentRepository;
        _userRepository = userRepository;
    }

    public IActionResult OnGet(int id)
    {
        News = _newsRepository.GetByID(id);

        if (News == null)
            return NotFound();

        _newsRepository.IncrementViews(id);
        News.Views += 1;

        Genre = _genreRepository.GetByID(News.GenreID);
        News.Author = News.AuthorID.HasValue ? _userRepository.GetByID(News.AuthorID.Value) : null;

        Comments = _commentRepository.GetByNewsIdWithUser(id);

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