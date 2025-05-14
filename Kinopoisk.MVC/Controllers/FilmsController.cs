using AutoMapper;
using Kinopoisk.MVC.Models;
using Kinopoisk.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Kinopoisk.MVC.Controllers;

public class FilmsController : Controller
{
    private readonly IFilmsService _filmsService;
    private readonly IMapper _mapper;
    private readonly ILogger<FilmsController> _logger;

    public FilmsController(IFilmsService filmsService, IMapper mapper, ILogger<FilmsController> logger)
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
        var filmsViewModel = _mapper.Map<IEnumerable<FilmsViewModel>>(filmDtos);
        return Json(filmsViewModel);
    }
}
