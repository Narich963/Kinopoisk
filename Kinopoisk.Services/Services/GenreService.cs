using AutoMapper;
using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Interfaces.Repositories;
using Kinopoisk.Core.Interfaces.Services;
using Kinopoisk.MVC.Models;
using System.Linq.Expressions;

namespace Kinopoisk.Services.Services;

public class GenreService : BaseService<Genre, GenreDTO, DataTablesRequestModel>, IGenreService
{
    private readonly IUnitOfWork _uow;
    private readonly IRepository<Genre, DataTablesRequestModel> _repository;
    public GenreService(IUnitOfWork uow, IMapper mapper) : base(uow, mapper)
    {
        _uow = uow;
        _repository = _uow.GetRepository<Genre, DataTablesRequestModel>();
    }

    public async override Task<Result> DeleteAsync(int? id)
    {
        if (!id.HasValue)
            return Result.Failure("Id is null");

        var genre = await _repository.GetByIdAsync(id.Value, includes: g => g.Films);

        if (genre.IsFailure)
            return Result.Failure(genre.Error);

        if (genre.Value.Films.Any())
            return Result.Failure("You can't delete genre that has films");

        return await base.DeleteAsync(id);
    }
}
