using System.ComponentModel.DataAnnotations;
using AvenaCore.Entities;
using AvenaCore.Repositories;
using AvenaWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace AvenaWeb.Controllers;

[ApiController]
[Route("api/admin/news")]
[Authorize(Policy = "AdminOnly")]
[AutoValidateAntiforgeryToken]
public class AdminNewsController : ControllerBase
{
    private readonly INewsRepository _newsRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IAuthorRepository _authorRepository;

    public AdminNewsController(
        INewsRepository newsRepository,
        IGenreRepository genreRepository,
        IAuthorRepository authorRepository)
    {
        _newsRepository = newsRepository;
        _genreRepository = genreRepository;
        _authorRepository = authorRepository;
    }

    [HttpGet]
    public IActionResult GetAll() => Ok(_newsRepository.GetAllForAdmin());

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var article = _newsRepository.GetByIDForAdmin(id);
        return article == null ? NotFound() : Ok(article);
    }

    [HttpPost]
    public IActionResult Create([FromBody] NewsWriteRequest request)
    {
        var validation = ValidateRequest(request, null, out var title, out var body, out var slug, out var summary, out var image);
        if (validation != null)
            return validation;

        var now = DateTime.UtcNow;
        var article = new News
        {
            Title = title,
            Info = body,
            Summary = summary,
            Slug = slug,
            Status = NewsStatuses.Draft,
            Image = image,
            Views = 0,
            CountOfLikes = 0,
            GenreID = request.GenreId,
            AuthorID = request.AuthorId,
            DateOfPost = now,
            PublishedAt = null,
            CreatedAt = now,
            UpdatedAt = now
        };

        try
        {
            article.ID = _newsRepository.Create(article);
            return Created($"/api/admin/news/{article.ID}", ToResponse(article));
        }
        catch (PostgresException exception) when (exception.SqlState == "23505")
        {
            return Conflict(new { error = "That article slug is already in use. Choose a different slug." });
        }
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] NewsWriteRequest request)
    {
        var article = _newsRepository.GetByIDForAdmin(id);
        if (article == null)
            return NotFound();

        var validation = ValidateRequest(request, id, out var title, out var body, out var slug, out var summary, out var image);
        if (validation != null)
            return validation;

        article.Title = title;
        article.Info = body;
        article.Summary = summary;
        article.Slug = slug;
        article.Image = image;
        article.GenreID = request.GenreId;
        article.AuthorID = request.AuthorId;
        article.UpdatedAt = DateTime.UtcNow;

        try
        {
            _newsRepository.UpdateArticle(article);
            return Ok(ToResponse(article));
        }
        catch (PostgresException exception) when (exception.SqlState == "23505")
        {
            return Conflict(new { error = "That article slug is already in use. Choose a different slug." });
        }
    }

    [HttpPost("{id:int}/publish")]
    public IActionResult Publish(int id)
    {
        var article = _newsRepository.GetByIDForAdmin(id);
        if (article == null)
            return NotFound();

        if (string.IsNullOrWhiteSpace(article.Title) || string.IsNullOrWhiteSpace(article.Info))
            return BadRequest(new { error = "A title and article body are required before publishing." });

        if (_genreRepository.GetByID(article.GenreID) == null)
            return BadRequest(new { error = "Choose a valid topic before publishing." });

        var slug = string.IsNullOrWhiteSpace(article.Slug)
            ? NewsSlug.FromTitle(article.Title)
            : article.Slug!;
        if (!NewsSlug.IsValid(slug))
            return BadRequest(new { error = "The article slug is invalid. Use letters, numbers, and hyphens." });
        if (_newsRepository.SlugExists(slug, id))
            return Conflict(new { error = "That article slug is already in use. Choose a different slug." });

        try
        {
            if (!string.Equals(article.Slug, slug, StringComparison.Ordinal))
            {
                article.Slug = slug;
                article.UpdatedAt = DateTime.UtcNow;
                _newsRepository.UpdateArticle(article);
            }

            _newsRepository.PublishArticle(id, DateTime.UtcNow);
            var published = _newsRepository.GetByIDForAdmin(id);
            return Ok(ToResponse(published!));
        }
        catch (PostgresException exception) when (exception.SqlState == "23505")
        {
            return Conflict(new { error = "That article slug is already in use. Choose a different slug." });
        }
    }

    private IActionResult? ValidateRequest(
        NewsWriteRequest request,
        int? exceptId,
        out string title,
        out string body,
        out string slug,
        out string? summary,
        out string? image)
    {
        title = request.Title?.Trim() ?? string.Empty;
        body = request.BodyMarkdown?.Trim() ?? string.Empty;
        summary = string.IsNullOrWhiteSpace(request.Summary) ? null : request.Summary.Trim();
        image = string.IsNullOrWhiteSpace(request.ImageUrl) ? null : request.ImageUrl.Trim();
        slug = string.IsNullOrWhiteSpace(request.Slug)
            ? NewsSlug.FromTitle(title)
            : request.Slug.Trim().ToLowerInvariant();

        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(body))
            return BadRequest(new { error = "A title and article body are required." });

        if (!NewsSlug.IsValid(slug))
            return BadRequest(new { error = "The slug may contain letters, numbers, and hyphens only." });

        if (_newsRepository.SlugExists(slug, exceptId))
            return Conflict(new { error = "That article slug is already in use. Choose a different slug." });

        if (_genreRepository.GetByID(request.GenreId) == null)
            return BadRequest(new { error = "Choose a valid topic." });

        if (request.AuthorId.HasValue && _authorRepository.GetByID(request.AuthorId.Value) == null)
            return BadRequest(new { error = "Choose a valid author." });

        if (!IsValidImageUrl(image))
            return BadRequest(new { error = "Use an HTTPS image URL or a path inside /images/." });

        return null;
    }

    private static bool IsValidImageUrl(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return true;

        if (value.StartsWith("/images/", StringComparison.Ordinal)
            && !value.Contains("..", StringComparison.Ordinal)
            && HasSafeUrlCharacters(value))
            return true;

        return HasSafeUrlCharacters(value)
            && Uri.TryCreate(value, UriKind.Absolute, out var uri)
            && uri.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase);
    }

    private static bool HasSafeUrlCharacters(string value) =>
        value.All(character => char.IsLetterOrDigit(character) || "-._~:/?#[]@!$&*+,=%".Contains(character));

    private static object ToResponse(News article) => new
    {
        id = article.ID,
        slug = article.Slug,
        status = article.Status,
        publishedAt = article.PublishedAt
    };
}

public class NewsWriteRequest
{
    [Required, StringLength(240)]
    public string? Title { get; set; }

    [StringLength(180)]
    public string? Slug { get; set; }

    [StringLength(320)]
    public string? Summary { get; set; }

    [Required, StringLength(100000)]
    public string? BodyMarkdown { get; set; }

    [Range(1, int.MaxValue)]
    public int GenreId { get; set; }

    [StringLength(2048)]
    public string? ImageUrl { get; set; }

    public int? AuthorId { get; set; }
}
