using AutoMapper;
using Kinopoisk.Core.Filters;
using Kinopoisk.Core.Interfaces.Services;
using Kinopoisk.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kinopoisk.MVC.Pages.Countries;

[Authorize(Roles = "admin")]
[IgnoreAntiforgeryToken]
public class IndexModel : PageModel
{
    private readonly ICountryService _countryService;
    private readonly IMapper _mapper;
    public IndexModel(ICountryService countryService, IMapper mapper)
    {
        _countryService = countryService;
        _mapper = mapper;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostGetCountriesAsync([FromBody] DataTablesRequestModel request)
    {
        var countries = await _countryService.GetPagedAsync(request);

        var result = new DataTablesResult<CountryViewModel>
        {
            Draw = request.Draw,
            RecordsTotal = countries.RecordsTotal,
            RecordsFiltered = countries.RecordsFiltered,
            Data = _mapper.Map<List<CountryViewModel>>(countries.Data),
        };
        return new JsonResult(result);
    }
    public async Task<IActionResult> OnPostDeleteCountryAsync(int? id)
    {
        var result = await _countryService.DeleteAsync(id);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return new JsonResult(new { success = true });
    }
}
