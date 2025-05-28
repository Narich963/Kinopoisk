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

        List<int> genreIds = new();
        if (isAddGenres)
        {
            genreIds = Film.SelectedGenreIds
                .Except(Film.Genres.Select(g => g.GenreId))
                .ToList();
        }
        else
        {
            genreIds = Film.Genres.Select(g => g.GenreId)
                .Except(Film.SelectedGenreIds)
                .ToList();
        }

        foreach (var genreId in genreIds)
        {
            var genreResult = isAddGenres 
                ? await _filmService.AddGenreToFilm(Film.Id, genreId) 
                : await _filmService.RemoveGenreFromFilm(Film.Id, genreId);

            if (genreResult.IsFailure)
            {
                ModelState.AddModelError(string.Empty, genreResult.Error);
                return Page();
            }
        }

        bool isAddActors = Film.SelectedActorIds.Count > Film.Actors.Count;

        List<int> actorIds = new();
        if (isAddActors)
        {
            actorIds = Film.SelectedActorIds
                .Except(Film.Actors.Select(a => a.FilmEmployeeId))
                .ToList();
        }
        else
        {
            actorIds = Film.Actors.Select(a => a.FilmEmployeeId)
                .Except(Film.SelectedActorIds)
                .ToList();
        }
        foreach (var actorId in actorIds)
        {
            var actorResult = isAddActors
                ? await _filmService.AddActorToFilm(Film.Id, actorId)
                : await _filmService.RemoveEmployeeFromFilm(Film.Id, actorId);

            if (actorResult.IsFailure)
            {
                ModelState.AddModelError(string.Empty, actorResult.Error);
                return Page();
            }
        }

        await _filmService.SaveChangesAsync();
        return RedirectToPage("./Index");
    }
}
