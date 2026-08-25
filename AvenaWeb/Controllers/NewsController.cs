using AvenaCore.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AvenaWeb.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewsController : ControllerBase
{
    private readonly INewsRepository _newsRepository;

    public NewsController(INewsRepository newsRepository)
    {
        _newsRepository = newsRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var news = _newsRepository.GetAll();

        return Ok(news);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var news = _newsRepository.GetByID(id);

        if (news == null)
            return NotFound();

        return Ok(news);
    }
}