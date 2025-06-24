using AutoMapper;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Interfaces.Services;
using Kinopoisk.WebApi.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Kinopoisk.WebApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class FilmEmployeeController : Controller
{
    private readonly IFilmEmployeeService _employeeService;
    private readonly IMapper _mapper;

    public FilmEmployeeController(IFilmEmployeeService employeeService, IMapper mapper)
    {
        _employeeService = employeeService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var employeesResult = await _employeeService.GetPagedAsync(new());
        return Ok(new { employees = _mapper.Map<List<FilmEmployeeResponse>>(employeesResult.Data) });
    }

    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        var employeeResult = await _employeeService.GetByIdAsync(id);
        if (employeeResult.IsSuccess)
            return Ok(new { result = _mapper.Map<FilmEmployeeResponse>(employeeResult.Value) });
        return BadRequest(new { message = employeeResult.Error });
    }

    [HttpPost]
    public async Task<IActionResult> Create(string name)
    {
        var newEmployee = new FilmEmployeeDTO
        {
            Name = name
        };
        var result = await _employeeService.AddAsync(newEmployee);
        if (result.IsSuccess)
            return Ok(new { employee = _mapper.Map<FilmEmployeeResponse>(result.Value) });
        return BadRequest(new { message = result.Error });
    }

    [HttpPut]
    public async Task<IActionResult> Edit(int id, string name)
    {
        var employee = new FilmEmployeeDTO
        {
            Id = id,
            Name = name
        };
        var result = await _employeeService.UpdateAsync(employee);
        if (result.IsSuccess)
            return Ok(new { employee = _mapper.Map<FilmEmployeeResponse>(result.Value) });
        return BadRequest(new { message = result.Error });
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _employeeService.DeleteAsync(id);
        if (result.IsSuccess)
            return Ok(new { message = "Employee deleted successfully" });
        return BadRequest(new { message = result.Error });
    }
}
