using AutoMapper;
using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Enitites;
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
    public FilmsViewModel Film { get; set; }

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
        return RedirectToPage("./Index");
    }
}
