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

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnGetGetFilmsAsync(FilmsFilterDTO dto)
    {
        var films = await _filmService.GetFilteredAsync(dto);
        var filmsViewModel = _mapper.Map<List<FilmsViewModel>>(films);
        return new JsonResult(filmsViewModel);
    }
}
