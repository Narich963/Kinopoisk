using AutoMapper;
using Kinopoisk.Core.DTO;
using Kinopoisk.Services.Interfaces;
using Kinopoisk.WebApi.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Kinopoisk.WebApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class FilmController : Controller
{
    private readonly IFilmService _filmsService;
    private readonly IMapper _mapper;

    public FilmController(IFilmService filmsService, IMapper mapper)
    {
        _filmsService = filmsService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var filmDtos = await _filmsService.GetAllAsync();
        var filmsResponse = _mapper.Map<IEnumerable<FilmResponse>>(filmDtos);
        return Ok(new {films = filmsResponse});
    }

    [HttpGet]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _filmsService.GetByIdAsync(id);
        if (result.IsSuccess)
        {
            var filmResponse = _mapper.Map<FilmResponse>(result.Value);
            return Ok(filmResponse);
        }
        return NotFound(new { message = result.Error });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] FilmCreateRequest request)
    {
        var filmDto = _mapper.Map<FilmDTO>(request);
        var result = await _filmsService.AddOrEditAsync(filmDto, true);
        if (result.IsSuccess)
        {
            var filmResponse = _mapper.Map<FilmResponse>(result.Value);
            return Ok( new { id = filmResponse.Id, message = "Film created successfully" });
        }
        return BadRequest(new { message = result.Error });
    }
}
