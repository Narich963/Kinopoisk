using AutoMapper;
using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Interfaces.Services;
using Kinopoisk.MVC.Models;
using Kinopoisk.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kinopoisk.MVC.Pages.FilmEmployees;

public class AddOrEditModel : PageModel
{
    private readonly IFilmEmployeeService _employeesService;
    private readonly IMapper _mapper;

    public AddOrEditModel(IFilmEmployeeService employeesService, IMapper mapper)
    {
        _employeesService = employeesService;
        _mapper = mapper;
    }

    [BindProperty]
    public FilmEmployeeViewModel FilmEmployee { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        Result<FilmEmployeeDTO> employeeResult = null;
        if (id.HasValue)
        {
            employeeResult = await _employeesService.GetByIdAsync(id);
            if (employeeResult.IsFailure)
            {
                ModelState.AddModelError(string.Empty, employeeResult.Error);
                return NotFound();
            }
        }

        FilmEmployee = id.HasValue
            ? _mapper.Map<FilmEmployeeViewModel>(employeeResult.Value)
            : new FilmEmployeeViewModel();

        FilmEmployee.IsNew = !id.HasValue;

        return Page();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();
        var genre = _mapper.Map<FilmEmployeeDTO>(FilmEmployee);

        Result<FilmEmployeeDTO> result = null;

        if (FilmEmployee.IsNew.Value)
            result = await _employeesService.AddAsync(genre);
        else
            result = await _employeesService.UpdateAsync(genre);

        if (result.IsFailure)
        {
            ModelState.AddModelError(string.Empty, result.Error);
            return Page();
        }
        return RedirectToPage("./Index");
    }
}
