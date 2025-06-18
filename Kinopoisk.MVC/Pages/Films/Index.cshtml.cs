using AutoMapper;
using Kinopoisk.Core.Filters;
using Kinopoisk.Core.Interfaces.Services;
using Kinopoisk.MVC.Models;
using Kinopoisk.Services.Interfaces;
using Kinopoisk.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuestPDF.Fluent;

namespace Kinopoisk.MVC.Pages.Films;

[AllowAnonymous]
[IgnoreAntiforgeryToken]
public class IndexModel : PageModel
{
    private readonly IFilmService _filmService;
    private readonly IRatingService _ratingService;
    private readonly IOmdbService _omdbService;
    private readonly IMapper _mapper;
    private readonly IDocumentService _documentService;
    private const string ADMIN_ROlE = "admin";

    public IndexModel(IFilmService filmService, IMapper mapper, IOmdbService omdbService, IRatingService ratingService, IDocumentService documentService)
    {
        _filmService = filmService;
        _mapper = mapper;
        _omdbService = omdbService;
        _ratingService = ratingService;
        _documentService = documentService;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostGetFilmsAsync([FromBody] FilmFilter model)
    {
        if (model == null)
            return BadRequest("Model is null");

        var result = await _filmService.GetPagedAsync(model);

        if (result.Data.Any(r => r.SitesRating == 0))
        {
            foreach (var film in result.Data)
            {
                var rating = await _ratingService.CalculateSitesRating(film.Id);
                if (rating.IsSuccess)
                {
                    film.SitesRating = rating.Value;
                }
                else
                {
                    film.SitesRating = 0;
                }
            }
        }

        DocumentService.Films = result.Data.ToList();

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
        if (!User.IsInRole(ADMIN_ROlE))
            return Unauthorized();

        var result = await _filmService.DeleteAsync(id);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return new JsonResult(new { success = true });
    }

    public async Task<IActionResult> OnPostImportFilmAsync([FromBody] string idOrTitle)
    {
        if (!User.IsInRole(ADMIN_ROlE))
            return Unauthorized();

        if (string.IsNullOrWhiteSpace(idOrTitle))
            return BadRequest("Title cannot be empty");
        
        var filmDto = await _omdbService.ImportFilm(idOrTitle);
        if (filmDto.IsFailure)
            return BadRequest(new {message = filmDto.Error});

        var filmViewModel = _mapper.Map<FilmsViewModel>(filmDto.Value);
        return new JsonResult(new {success = true}) ;
    }

    public void OnGetExportToPdf()
    {
        if (!User.IsInRole(ADMIN_ROlE))
            throw new UnauthorizedAccessException("You do not have permission to export to PDF.");

        _documentService.GeneratePdfAndShow();
    }
    public IActionResult OnGetExportToExcel()
    {
        if (!User.IsInRole(ADMIN_ROlE))
            return Unauthorized();

        var excelBytes = _documentService.ExportToExcel();
        return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "films.xlsx");
    }
}
