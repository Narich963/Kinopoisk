using AutoMapper;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Filters;
using Kinopoisk.MVC.Models;
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

    [HttpPost]
    public async Task<IActionResult> GetPagedAsync([FromBody] FilmsFilteredRequest request)
    {
        var filter = new FilmFilter
        {
            Draw = 1,
            Start = request.PageSize * request.Page,
            Length = request.PageSize,
            Columns = new()
            {
                new DataTablesRequestModel.ColumnModel
                {
                    Data = request.Column,
                    Name = "",
                    Orderable = true
                }
            },
            Order = new()
            {
                new DataTablesRequestModel.OrderModel
                {
                    Column = 0,
                    Dir = request.Direction
                }
            },
            Search = new()
            {
                Value = request.Search,
                Regex = false
            },
            Name = request.Name,
            Year = request.Year,
            Country = request.Country,
            Director = request.Director,
            Actor = request.Actor
        };

        var filmsResult = await _filmsService.GetPagedAsync(filter);
        return Ok(new { result = _mapper.Map<FilmResponse>(filmsResult.Data) });
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

    [HttpPatch]
    public async Task<IActionResult> Edit(int id, [FromBody] FilmCreateRequest request)
    {
        var filmDto = _mapper.Map<FilmDTO>(request);
        filmDto.Id = id;
        var result = await _filmsService.AddOrEditAsync(filmDto, false);
        if (result.IsSuccess)
        {
            var filmResponse = _mapper.Map<FilmResponse>(result.Value);
            return Ok(new { id = filmResponse.Id, message = "Film edited successfully" });
        }
        return BadRequest(new { message = result.Error });
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _filmsService.DeleteAsync(id);
        if (result.IsSuccess)
            return Ok(new { message = "Film deleted successfully" });
        return BadRequest(new { message = result.Error });
    }
}
