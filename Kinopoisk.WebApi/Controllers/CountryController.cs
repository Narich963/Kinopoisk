using AutoMapper;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Interfaces.Services;
using Kinopoisk.WebApi.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Kinopoisk.WebApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class CountryController : Controller
{
    private readonly ICountryService _countryService;
    private readonly IMapper _mapper;

    public CountryController(ICountryService countryService, IMapper mapper)
    {
        _countryService = countryService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var countriesResult = await _countryService.GetPagedAsync(new());
        return Ok(new { countries = _mapper.Map<List<CountryResponse>>(countriesResult.Data) });
    }

    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        var countryResult = await _countryService.GetByIdAsync(id);
        if (countryResult.IsSuccess)
            return Ok(new { country = _mapper.Map<CountryResponse>(countryResult.Value) });
        return BadRequest(new { message = countryResult.Error });
    }

    [HttpPost]
    public async Task<IActionResult> Create(string name)
    {
        var newCountry = new CountryDTO
        {
            Name = name
        };
        var countryResult = await _countryService.AddAsync(newCountry);
        if (countryResult.IsSuccess)
            return Ok(new { country = _mapper.Map<CountryResponse>(countryResult.Value) });
        return BadRequest(new { message = countryResult.Error });
    }

    [HttpPut]
    public async Task<IActionResult> Edit(int id, string name)
    {
        var country = new CountryDTO
        {
            Id = id,
            Name = name
        };
        var countryResult = await _countryService.UpdateAsync(country);
        if (countryResult.IsSuccess)
            return Ok(new { country = _mapper.Map<CountryResponse>(countryResult.Value) });
        return BadRequest(new { message = countryResult.Error });
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _countryService.DeleteAsync(id);
        if (result.IsSuccess)
            return Ok(new { message = "Country deleted successfully" });
        return BadRequest(new { message = result.Error });
    }
}
