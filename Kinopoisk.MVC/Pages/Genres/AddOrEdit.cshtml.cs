using AutoMapper;
using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Interfaces.Services;
using Kinopoisk.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kinopoisk.MVC.Pages.Genres;

public class AddOrEditModel : PageModel
{
    private readonly IGenreService _genreService;
    private readonly IMapper _mapper;

    public AddOrEditModel(IGenreService genreService, IMapper mapper)
    {
        _genreService = genreService;
        _mapper = mapper;
    }

    [BindProperty]
    public GenreViewModel Genre { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        Result<GenreDTO> genreResult = null;
        if (id.HasValue)
        {
            genreResult = await _genreService.GetByIdAsync(id);
            if (genreResult.IsFailure)
            {
                ModelState.AddModelError(string.Empty, genreResult.Error);
                return NotFound();
            }
        }

        Genre = id.HasValue
            ? _mapper.Map<GenreViewModel>(genreResult.Value)
            : new GenreViewModel();

        Genre.IsNew = !id.HasValue;

        return Page();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();
        var genre = _mapper.Map<GenreDTO>(Genre);

        Result<GenreDTO> result = null;

        if (Genre.IsNew.Value)
            result = await _genreService.AddAsync(genre);
        else
            result = await _genreService.UpdateAsync(genre);

        if (result.IsFailure)
        {
            ModelState.AddModelError(string.Empty, result.Error);
            return Page();
        }
        return RedirectToPage("./Index");
    }
}
