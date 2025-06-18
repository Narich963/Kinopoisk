using AutoMapper;
using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Interfaces.Repositories;
using Kinopoisk.Core.Interfaces.Services;
using Kinopoisk.MVC.Models;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Kinopoisk.Services.Services;

public class GenreService : BaseService<Genre, GenreDTO, DataTablesRequestModel>, IGenreService
{
    private readonly IUnitOfWork _uow;
    private readonly ILogger<GenreService> _logger;
    private readonly IRepository<Genre, DataTablesRequestModel> _repository;
    public GenreService(IUnitOfWork uow, IMapper mapper, ILogger<GenreService> logger) : base(uow, mapper, logger)
    {
        _uow = uow;
        _repository = _uow.GetGenericRepository<Genre, DataTablesRequestModel>();
        _logger = logger;
    }

    public async override Task<Result> DeleteAsync(int? id)
    {
        if (!id.HasValue)
        {
            _logger.Log(LogLevel.Error, "Id is null in DeleteAsync method");
            return Result.Failure("Id is null");
        }

        var genre = await _repository.GetByIdAsync(id.Value, includes: g => g.Films);
        if (genre.IsFailure)
        {
            _logger.Log(LogLevel.Error, "An error occurred while trying to delete Genre with Id - {Id}. Message: {Error}", id.Value, genre.Error);
            return Result.Failure(genre.Error);
        }

        if (genre.Value.Films.Any())
        {
            _logger.Log(LogLevel.Warning, "Attempt to delete Genre with Id - {Id} that has associated films", id.Value);
            return Result.Failure("You can't delete genre that has films");
        }    
        return await base.DeleteAsync(id);
    }
}
