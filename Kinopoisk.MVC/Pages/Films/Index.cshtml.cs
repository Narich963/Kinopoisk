using AutoMapper;
using Kinopoisk.Core.Filters;
using Kinopoisk.MVC.Models;
using Kinopoisk.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kinopoisk.MVC.Pages.Films;

[IgnoreAntiforgeryToken]
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

    public async Task<IActionResult> OnPostGetFilmsAsync([FromBody] FilmFilter model)
    {
        if (model == null)
            return BadRequest("Model is null");

        var result = await _filmService.GetPagedAsync(model);

        var filmsPaged = new DataTablesResult<FilmsViewModel>
        {
            Draw = result.Draw,
            RecordsTotal = result.RecordsTotal,
            RecordsFiltered = result.RecordsFiltered,
            Data = _mapper.Map<List<FilmsViewModel>>(result.Data)
        };

        return new JsonResult(filmsPaged);
    }

    public async Task<IActionResult> OnPostDeleteFilmAsync(int? id)
    {
        var result = await _filmService.DeleteAsync(id);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return new JsonResult(new { success = true });
    }
}
