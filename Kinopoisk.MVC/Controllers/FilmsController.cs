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
    public async Task<IActionResult> Index()
    {
        var filmDtos = await _filmsService.GetAllAsync();
        var filmsResponse = _mapper.Map<IEnumerable<FilmsViewModel>>(filmDtos);
        return View(filmsResponse);
    }
}
