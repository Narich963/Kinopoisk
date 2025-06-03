using AutoMapper;
using Kinopoisk.Core.Filters;
using Kinopoisk.Core.Interfaces.Services;
using Kinopoisk.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kinopoisk.MVC.Pages.FilmEmployees;

[Authorize(Roles = "admin")]
[IgnoreAntiforgeryToken]
public class IndexModel : PageModel
{
    private readonly IFilmEmployeeService _employeesService;
    private readonly IMapper _mapper;

    public IndexModel(IFilmEmployeeService actorService, IMapper mapper)
    {
        _employeesService = actorService;
        _mapper = mapper;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostGetEmployeesAsync([FromBody] DataTablesRequestModel request)
    {
        if (request == null)
            return BadRequest("Invalid request");

        var filmEmployees = await _employeesService.GetPagedAsync(request);

        var result = new DataTablesResult<FilmEmployeeViewModel>
        {
            Draw = request.Draw,
            RecordsTotal = filmEmployees.RecordsTotal,
            RecordsFiltered = filmEmployees.RecordsFiltered,
            Data = _mapper.Map<List<FilmEmployeeViewModel>>(filmEmployees.Data)
        };
        return new JsonResult(result);
    }
    public async Task<IActionResult> OnPostDeleteFilmEmployeeAsync(int? id)
    {
        var result = await _employeesService.DeleteAsync(id);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return new JsonResult(new { success = true });
    }
}
