using AutoMapper;
using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.DTO.Localization;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Enums;
using Kinopoisk.Core.Interfaces.Services;
using Kinopoisk.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kinopoisk.MVC.Pages.Genres;

[Authorize(Roles = "admin")]
public class AddOrEditModel : PageModel
{
    private readonly IGenreService _genreService;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;

    public AddOrEditModel(IGenreService genreService, IMapper mapper, ILocalizationService localizationService)
    {
        _genreService = genreService;
        _mapper = mapper;
        _localizationService = localizationService;
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

        if (!Genre.IsNew.Value)
            Genre.NameLocalizations = _mapper.Map<List<LocalizationViewModel>>(await _localizationService.GetLocalizations(Genre.Id));
        else
            Genre.NameLocalizations = _mapper.Map<List<LocalizationViewModel>>(await _localizationService.GetEmptyLocalizations(PropertyEnum.Name));

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

        var localizationDtos = _mapper.Map<List<LocalizationDTO>>(Genre.NameLocalizations);
        await _localizationService.UpdateLocalizations(localizationDtos, result.Value.Id);

        if (result.IsFailure)
        {
            ModelState.AddModelError(string.Empty, result.Error);
            return Page();
        }
        return RedirectToPage("./Index");
    }
}
