using AutoMapper;
using Kinopoisk.MVC.Models;
using Kinopoisk.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Kinopoisk.MVC.Controllers;

public class FilmController : Controller
{
    private readonly IFilmService _filmsService;
    private readonly IMapper _mapper;
    private readonly ILogger<FilmController> _logger;

    public FilmController(IFilmService filmsService, IMapper mapper, ILogger<FilmController> logger)
    {
        _filmsService = filmsService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetFilms(string? name, int? year, string? country, string? actorName, string? director)
    {
        var filmDtos = await _filmsService.GetFilteredAsync(name, year, country, actorName, director);
        var filmsViewModel = _mapper.Map<List<FilmsViewModel>>(filmDtos);
        return Json(filmsViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        var filmDto = await _filmsService.GetByIdAsync(id);
        if (filmDto.IsFailure)
        {
            _logger.LogError(filmDto.Error);
            return NotFound();
        }
        var filmVm = _mapper.Map<FilmsViewModel>(filmDto.Value);
        return View(filmVm);
    }
}
