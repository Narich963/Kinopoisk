using AutoMapper;
using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.DTO.Localization;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Enums;
using Kinopoisk.Core.Interfaces.Services;
using Kinopoisk.MVC.Models;
using Kinopoisk.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kinopoisk.MVC.Pages.Films;

[Authorize(Roles = "admin")]
public class AddOrEditModel : PageModel
{
    private readonly IFilmService _filmService;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;

    public AddOrEditModel(IFilmService filmService, IMapper mapper, ILocalizationService localizationService)
    {
        _filmService = filmService;
        _mapper = mapper;
        _localizationService = localizationService;
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

        if (!Film.IsNew.Value)
        {
            Film.NameLocalizations = _mapper.Map<List<LocalizationViewModel>>(await _localizationService.GetLocalizations(Film.Id, PropertyEnum.Name));
            Film.DescriptionLocalizations = _mapper.Map<List<LocalizationViewModel>>(await _localizationService.GetLocalizations(Film.Id, PropertyEnum.Description));
        }
        else
        {
            Film.NameLocalizations = _mapper.Map<List<LocalizationViewModel>>(await _localizationService.GetEmptyLocalizations(PropertyEnum.Name));
            Film.DescriptionLocalizations = _mapper.Map<List<LocalizationViewModel>>(await _localizationService.GetEmptyLocalizations(PropertyEnum.Description));
        }
        return Page();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var film = _mapper.Map<FilmDTO>(Film);
        var result = await _filmService.AddOrEditAsync(film, Film.IsNew);

        var nameLocalizations = _mapper.Map<List<LocalizationDTO>>(Film.NameLocalizations);
        await _localizationService.UpdateLocalizations(nameLocalizations, result.Value.Id);

        var descriptionLocalizations = _mapper.Map<List<LocalizationDTO>>(Film.DescriptionLocalizations);
        await _localizationService.UpdateLocalizations(descriptionLocalizations, result.Value.Id);

        if (result.IsFailure)
        {
            ModelState.AddModelError(string.Empty, result.Error);
            return Page();
        }
        return RedirectToPage("./Index");
    }
}
