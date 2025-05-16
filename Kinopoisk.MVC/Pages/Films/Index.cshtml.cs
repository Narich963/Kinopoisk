using AutoMapper;
using Kinopoisk.Core.DTO;
using Kinopoisk.MVC.Models;
using Kinopoisk.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kinopoisk.MVC.Pages.Films;

public class IndexModel : PageModel
{
    private readonly IFilmService _filmService;
    private readonly IMapper _mapper;

    public IndexModel(IFilmService filmService, IMapper mapper)
    {
        _filmService = filmService;
        _mapper = mapper;
    }

    [BindProperty(SupportsGet = true)]
    public FilmsFilterDTO FilterModel { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnGetGetFilmsAsync()
    {
        var films = await _filmService.GetFilteredAsync(FilterModel);
        var filmsViewModel = _mapper.Map<List<FilmsViewModel>>(films);
        return new JsonResult(filmsViewModel);
    }
}
