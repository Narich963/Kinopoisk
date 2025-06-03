using AutoMapper;
using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Interfaces.Repositories;
using Kinopoisk.Core.Interfaces.Services;
using Kinopoisk.MVC.Models;
using Microsoft.Extensions.Logging;

namespace Kinopoisk.Services.Services;

public class FilmEmployeeService : BaseService<FilmEmployee, FilmEmployeeDTO, DataTablesRequestModel>, IFilmEmployeeService
{
    private readonly IUnitOfWork _uow;
    private readonly ILogger<FilmEmployeeService> _logger;
    private readonly IRepository<FilmEmployee, DataTablesRequestModel> _repository;
    public FilmEmployeeService(IUnitOfWork uow, IMapper mapper, ILogger<FilmEmployeeService> logger) : base(uow, mapper, logger)
    {
        _uow = uow;
        _repository = _uow.GetRepository<FilmEmployee, DataTablesRequestModel>();
        _logger = logger;
    }

    public async override Task<Result> DeleteAsync(int? id)
    {
        if (!id.HasValue)
        {
            _logger.Log(LogLevel.Error, "Id is null in DeleteAsync method");
            return Result.Failure("Id is null");
        }

        var employee = await _repository.GetByIdAsync(id.Value, includes: e => e.ActorRoles);

        if (employee.IsFailure)
            return Result.Failure(employee.Error);

        if (employee.Value.ActorRoles.Any())
            return Result.Failure("You can't delete employee that has roles");

        return await base.DeleteAsync(id);
    }
}
