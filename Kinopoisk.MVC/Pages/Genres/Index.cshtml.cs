using AutoMapper;
using Kinopoisk.Core.Filters;
using Kinopoisk.Core.Interfaces.Services;
using Kinopoisk.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kinopoisk.MVC.Pages.Genres;

[IgnoreAntiforgeryToken]
public class IndexModel : PageModel
{
    private readonly IGenreService _genreService;
    private readonly IMapper _mapper;
    public IndexModel(IGenreService genreService, IMapper mapper)
    {
        _genreService = genreService;
        _mapper = mapper;
    }

    public void OnGet()
    {

    }

    public async Task<IActionResult> OnPostGetGenresAsync([FromBody] DataTablesRequestModel model)
    {
        if (model == null)
            return BadRequest();

        var genres = await _genreService.GetPagedAsync(model);
        var genresPaged = new DataTablesResult<GenreViewModel>
        {
            Draw = model.Draw,
            RecordsTotal = genres.RecordsTotal,
            RecordsFiltered = genres.RecordsFiltered,
            Data = _mapper.Map<List<GenreViewModel>>(genres.Data)
        };
        return new JsonResult(genresPaged);
    }

    public async Task<IActionResult> OnPostDeleteGenreAsync(int? id)
    {
        var result = await _genreService.DeleteAsync(id);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return new JsonResult(new { success = true });
    }
}
