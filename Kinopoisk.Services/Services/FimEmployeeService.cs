using AutoMapper;
using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Interfaces.Repositories;
using Kinopoisk.Core.Interfaces.Services;
using Kinopoisk.MVC.Models;

namespace Kinopoisk.Services.Services;

public class FimEmployeeService : BaseService<FilmEmployee, FilmEmployeeDTO, DataTablesRequestModel>, IFilmEmployeeService
{
    private readonly IUnitOfWork _uow;
    private readonly IRepository<FilmEmployee, DataTablesRequestModel> _repository;
    public FimEmployeeService(IUnitOfWork uow, IMapper mapper) : base(uow, mapper)
    {
        _uow = uow;
        _repository = _uow.GetRepository<FilmEmployee, DataTablesRequestModel>();
    }

    public async override Task<Result> DeleteAsync(int? id)
    {
        if (!id.HasValue)
            return Result.Failure("Id is null");

        var employee = await _repository.GetByIdAsync(id.Value, includes: e => e.ActorRoles);

        if (employee.IsFailure)
            return Result.Failure(employee.Error);

        if (employee.Value.ActorRoles.Any())
            return Result.Failure("You can't delete employee that has roles");

        return await base.DeleteAsync(id);
    }
}
