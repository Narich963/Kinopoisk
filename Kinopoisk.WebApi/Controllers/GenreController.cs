using AutoMapper;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Interfaces.Services;
using Kinopoisk.WebApi.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Kinopoisk.WebApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class GenreController : Controller
{
    private readonly IGenreService _genreService;
    private readonly IMapper _mapper;

    public GenreController(IGenreService genreService, IMapper mapper)
    {
        _genreService = genreService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var genresResult = await _genreService.GetPagedAsync(new());
        return Ok(new { genres = _mapper.Map<List<GenreResponse>>(genresResult.Data) });
    }

    [HttpGet]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _genreService.GetByIdAsync(id);
        if (result.IsSuccess)
            return Ok(new { genre = _mapper.Map<GenreResponse>(result.Value) });
        return BadRequest(new { message = result.Error });
    }

    [HttpPost]
    public async Task<IActionResult> Create(string name)
    {
        var newGenre = new GenreDTO
        {
            Name = name
        };
        var result = await _genreService.AddAsync(newGenre);
        if (result.IsSuccess)
            return Ok(new { genre = _mapper.Map<GenreResponse>(result.Value) });
        return BadRequest(new { message = result.Value });
    }

    [HttpPut]
    public async Task<IActionResult> Edit(int id, string name)
    {
        var genre = new GenreDTO
        {
            Id = id,
            Name = name
        };
        var result = await _genreService.UpdateAsync(genre);
        if (result.IsSuccess)
            return Ok(new { genre = _mapper.Map<GenreResponse>(result.Value) });
        return BadRequest(new { message = result.Error });
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _genreService.DeleteAsync(id);
        if (result.IsSuccess)
            return Ok(new { message = "Genre deleted successfully" });
        return BadRequest(new { message = result.Error });
    }
}
