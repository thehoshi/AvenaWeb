using System.ComponentModel.DataAnnotations;
using AvenaCore.Entities;
using AvenaCore.Repositories;
using AvenaWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
    private readonly IWebHostEnvironment _environment;

    public AdminNewsController(INewsRepository newsRepository, IGenreRepository genreRepository, IAuthorRepository authorRepository, IWebHostEnvironment environment)
    {
        _newsRepository = newsRepository;
        _genreRepository = genreRepository;
        _authorRepository = authorRepository;
        _environment = environment;
    }

    [HttpGet]
    public IActionResult GetAll() => Ok(_newsRepository.GetAllForAdmin());

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var article = _newsRepository.GetByIDForAdmin(id);
        return article == null ? NotFound() : Ok(article);
    }

    [HttpPost("topics")]
    public IActionResult CreateTopic([FromBody] CreateTopicRequest request)
    {
        var name = request.Name?.Trim() ?? string.Empty;
        if (name.Length is < 2 or > 80)
            return BadRequest(new { error = "A topic name must be between 2 and 80 characters." });

        int id;
        try
        {
            id = _genreRepository.GetOrCreate(name);
        }
        catch (PostgresException ex) when (ex.SqlState == "23505")
        {
            id = _genreRepository.GetOrCreate(name);
        }
        var topic = _genreRepository.GetByID(id);
        return Ok(new { id, name = topic?.NameGenre ?? name });
    }

    [HttpPost("image")]
    [RequestSizeLimit(5_500_000)]
    [RequestFormLimits(MultipartBodyLengthLimit = 5_500_000)]
    public async Task<IActionResult> UploadImage([FromForm] IFormFile? file, CancellationToken cancellationToken)
    {
        const long maximumBytes = 5 * 1024 * 1024;
        if (file == null || file.Length == 0) return BadRequest(new { error = "Choose an image file first." });
        if (file.Length > maximumBytes) return BadRequest(new { error = "Images must be 5 MB or smaller." });

        await using var input = file.OpenReadStream();
        using var memory = new MemoryStream();
        await input.CopyToAsync(memory, cancellationToken);
        var bytes = memory.ToArray();
        var extension = GetImageExtension(bytes);
        if (extension == null) return BadRequest(new { error = "Use a valid JPG, PNG, or WebP image." });

        var webRoot = _environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot");
        var directory = Path.Combine(webRoot, "images", "articles");
        Directory.CreateDirectory(directory);
        var fileName = $"{Guid.NewGuid():N}{extension}";
        await System.IO.File.WriteAllBytesAsync(Path.Combine(directory, fileName), bytes, cancellationToken);
        return Ok(new { imageUrl = $"/images/articles/{fileName}" });
    }

    [HttpPost]
    public IActionResult Create([FromBody] NewsWriteRequest request)
    {
        var invalid = ValidateRequest(request, null, out var title, out var body, out var slug, out var summary, out var image);
        if (invalid != null) return invalid;
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
            AuthorID = ResolveAuthorId(request.AuthorName),
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
        catch (PostgresException ex) when (ex.SqlState == "23505")
        {
            return Conflict(new { error = "That article slug is already in use. Choose a different slug." });
        }
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] NewsWriteRequest request)
    {
        var article = _newsRepository.GetByIDForAdmin(id);
        if (article == null) return NotFound();
        var invalid = ValidateRequest(request, id, out var title, out var body, out var slug, out var summary, out var image);
        if (invalid != null) return invalid;

        article.Title = title;
        article.Info = body;
        article.Summary = summary;
        article.Slug = slug;
        article.Image = image;
        article.GenreID = request.GenreId;
        article.AuthorID = ResolveAuthorId(request.AuthorName);
        article.UpdatedAt = DateTime.UtcNow;
        try
        {
            _newsRepository.UpdateArticle(article);
            return Ok(ToResponse(article));
        }
        catch (PostgresException ex) when (ex.SqlState == "23505")
        {
            return Conflict(new { error = "That article slug is already in use. Choose a different slug." });
        }
    }

    [HttpPost("{id:int}/publish")]
    public IActionResult Publish(int id)
    {
        var article = _newsRepository.GetByIDForAdmin(id);
        if (article == null) return NotFound();
        if (string.IsNullOrWhiteSpace(article.Title) || string.IsNullOrWhiteSpace(article.Info))
            return BadRequest(new { error = "A title and article body are required before publishing." });
        if (_genreRepository.GetByID(article.GenreID) == null)
            return BadRequest(new { error = "Choose a valid topic before publishing." });

        var slug = string.IsNullOrWhiteSpace(article.Slug) ? NewsSlug.FromTitle(article.Title) : article.Slug!;
        if (!NewsSlug.IsValid(slug)) return BadRequest(new { error = "The article slug is invalid." });
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
            return Ok(ToResponse(_newsRepository.GetByIDForAdmin(id)!));
        }
        catch (PostgresException ex) when (ex.SqlState == "23505")
        {
            return Conflict(new { error = "That article slug is already in use. Choose a different slug." });
        }
    }

    private IActionResult? ValidateRequest(NewsWriteRequest request, int? exceptId,
        out string title, out string body, out string slug, out string? summary, out string? image)
    {
        title = request.Title?.Trim() ?? string.Empty;
        body = request.BodyMarkdown?.Trim() ?? string.Empty;
        summary = string.IsNullOrWhiteSpace(request.Summary) ? null : request.Summary.Trim();
        image = string.IsNullOrWhiteSpace(request.ImageUrl) ? null : request.ImageUrl.Trim();
        slug = string.IsNullOrWhiteSpace(request.Slug) ? NewsSlug.FromTitle(title) : request.Slug.Trim().ToLowerInvariant();

        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(body))
            return BadRequest(new { error = "A title and article body are required." });
        if (!NewsSlug.IsValid(slug))
            return BadRequest(new { error = "The slug may contain letters, numbers, and hyphens only." });
        if (_newsRepository.SlugExists(slug, exceptId))
            return Conflict(new { error = "That article slug is already in use. Choose a different slug." });
        if (_genreRepository.GetByID(request.GenreId) == null)
            return BadRequest(new { error = "Choose a valid topic." });
        if (!string.IsNullOrWhiteSpace(request.AuthorName) && request.AuthorName.Trim().Length > 160)
            return BadRequest(new { error = "The byline must be 160 characters or fewer." });
        if (!IsValidImageUrl(image))
            return BadRequest(new { error = "Use an HTTPS image URL or a path inside /images/." });
        return null;
    }

    private static bool IsValidImageUrl(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return true;
        if (value.StartsWith("/images/", StringComparison.Ordinal)
            && !value.Contains("..", StringComparison.Ordinal) && HasSafeUrlCharacters(value)) return true;
        return HasSafeUrlCharacters(value) && Uri.TryCreate(value, UriKind.Absolute, out var uri)
            && uri.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase);
    }

    private static bool HasSafeUrlCharacters(string value) =>
        value.All(character => char.IsLetterOrDigit(character) || "-._~:/?#[]@!$&*+,=%".Contains(character));

    private int? ResolveAuthorId(string? authorName) =>
        string.IsNullOrWhiteSpace(authorName) ? null : _authorRepository.GetOrCreateByName(authorName);

    private static string? GetImageExtension(byte[] bytes)
    {
        if (bytes.Length >= 8 && bytes.AsSpan(0, 8).SequenceEqual(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 })) return ".png";
        if (bytes.Length >= 3 && bytes[0] == 0xFF && bytes[1] == 0xD8 && bytes[2] == 0xFF) return ".jpg";
        if (bytes.Length >= 12 && bytes.AsSpan(0, 4).SequenceEqual("RIFF"u8) && bytes.AsSpan(8, 4).SequenceEqual("WEBP"u8)) return ".webp";
        return null;
    }

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
    [Required, StringLength(240)] public string? Title { get; set; }
    [StringLength(180)] public string? Slug { get; set; }
    [StringLength(320)] public string? Summary { get; set; }
    [Required, StringLength(100000)] public string? BodyMarkdown { get; set; }
    [Range(1, int.MaxValue)] public int GenreId { get; set; }
    [StringLength(2048)] public string? ImageUrl { get; set; }
    [StringLength(160)] public string? AuthorName { get; set; }
}

public class CreateTopicRequest
{
    [Required, StringLength(80)] public string? Name { get; set; }
}
