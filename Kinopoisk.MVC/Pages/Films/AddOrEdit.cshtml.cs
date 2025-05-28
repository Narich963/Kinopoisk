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

        var deleteActors = Film.Actors.Where(a => a.IsForDeleting);
        foreach (var actor in deleteActors)
        {
            var actorResult = await _filmService.RemoveEmployeeFromFilm(Film.Id, actor.FilmEmployeeId);
            if (actorResult.IsFailure)
            {
                ModelState.AddModelError(string.Empty, actorResult.Error);
                return Page();
            }
        }
        Film.Actors = Film.Actors.Where(a => !a.IsForDeleting).ToList();

        var film = _mapper.Map<FilmDTO>(Film);

        Result<FilmDTO> result = null;

        if (Film.IsNew.Value)
            result = await _filmService.AddAsync(film);
        else
            result = await _filmService.UpdateAsync(film);

        if (result.IsFailure)
        {
            ModelState.AddModelError(string.Empty, result.Error);
            return Page();
        }
        await _filmService.SaveChangesAsync();
        return RedirectToPage("./Index");
    }
}
