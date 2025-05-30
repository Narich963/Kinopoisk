using AutoMapper;
using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Interfaces.Services;
using Kinopoisk.MVC.Models;
using Kinopoisk.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kinopoisk.MVC.Pages.Films;

public class AddOrEditModel : PageModel
{
    private readonly IFilmService _filmService;
    private readonly IMapper _mapper;

    public AddOrEditModel(IFilmService filmService, IMapper mapper)
    {
        _filmService = filmService;
        _mapper = mapper;
    }

    [BindProperty]
    public FilmsViewModel Film { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        Result<FilmDTO> filmResult = null;
        if (id.HasValue)
        {
            filmResult = await _filmService.GetByIdAsync(id);
            if (filmResult.IsFailure)
            {
                ModelState.AddModelError(string.Empty, filmResult.Error);
                return NotFound();
            }
        }

        Film = id.HasValue
            ? _mapper.Map<FilmsViewModel>(filmResult.Value)
            : new FilmsViewModel();

        Film.IsNew = !id.HasValue;

        return Page();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var film = _mapper.Map<FilmDTO>(Film);

        var result = Film.IsNew.Value 
            ? await _filmService.AddAsync(film) 
            : await _filmService.UpdateAsync(film);

        if (result.IsFailure)
        {
            ModelState.AddModelError(string.Empty, result.Error);
            return Page();
        }

        bool isAddGenres = Film.SelectedGenreIds.Count > Film.Genres.Count;
        var genreIds = isAddGenres
            ? Film.SelectedGenreIds
                .Except(Film.Genres.Select(g => g.GenreId))
                .ToList()
            : Film.Genres.Select(g => g.GenreId)
                .Except(Film.SelectedGenreIds)
                .ToList();

        var genresResult = await _filmService.UpdateFilmGenres(genreIds, Film.Id, isAddGenres);
        if (genresResult.IsFailure)
        {
            ModelState.AddModelError(string.Empty, genresResult.Error);
            return Page();
        }

        bool isAddActors = Film.SelectedActorIds.Count > Film.Actors.Count;
        var actorIds = isAddActors
            ? Film.SelectedActorIds
                .Except(Film.Actors.Select(a => a.FilmEmployeeId))
                .ToList()
            : Film.Actors.Select(a => a.FilmEmployeeId)
                .Except(Film.SelectedActorIds)
                .ToList();

        var actorsResult = await _filmService.UpdateFilmActors(actorIds, Film.Id, isAddActors);
        if (actorsResult.IsFailure)
        {
            ModelState.AddModelError(string.Empty, actorsResult.Error);
            return Page();
        }

        return RedirectToPage("./Index");
    }
}
