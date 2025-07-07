using AutoMapper;
using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.DTO.Localization;
using Kinopoisk.Core.Enums;
using Kinopoisk.Core.Interfaces.Services;
using Kinopoisk.MVC.Models;
using Kinopoisk.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kinopoisk.MVC.Pages.Countries;

[Authorize(Roles = "admin")]
[IgnoreAntiforgeryToken]
public class AddOrEditModel : PageModel
{
    private readonly ICountryService _countryService;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;

    public AddOrEditModel(ICountryService countryService, IMapper mapper, ILocalizationService localizationService)
    {
        _countryService = countryService;
        _mapper = mapper;
        _localizationService = localizationService;
    }

    [BindProperty]  
    public CountryViewModel Country { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        Result<CountryDTO> countryResult = null;
        if (id.HasValue)
        {
            countryResult = await _countryService.GetByIdAsync(id);
            if (countryResult.IsFailure)
            {
                ModelState.AddModelError(string.Empty, countryResult.Error);
                return NotFound();
            }
        }

        Country = id.HasValue
            ? _mapper.Map<CountryViewModel>(countryResult.Value)
            : new CountryViewModel();

        Country.IsNew = !id.HasValue;

        if (!Country.IsNew.Value)
        {
            Country.NameLocalizations = _mapper.Map<List<LocalizationViewModel>>(await _localizationService.GetLocalizations(Country.Id));
        }
        else
        {
            Country.NameLocalizations = _mapper.Map<List<LocalizationViewModel>>(await _localizationService.GetEmptyLocalizations(PropertyEnum.Name));
        }

        return Page();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();
        var genre = _mapper.Map<CountryDTO>(Country);

        Result<CountryDTO> result = null;

        if (Country.IsNew.Value)
            result = await _countryService.AddAsync(genre);
        else
            result = await _countryService.UpdateAsync(genre);

        var localizationDtos = _mapper.Map<List<LocalizationDTO>>(Country.NameLocalizations);
        await _localizationService.UpdateLocalizations(localizationDtos, result.Value.Id);

        if (result.IsFailure)
        {
            ModelState.AddModelError(string.Empty, result.Error);
            return Page();
        }
        return RedirectToPage("./Index");
    }
}
