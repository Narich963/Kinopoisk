using AutoMapper;
using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Interfaces.Repositories;
using Kinopoisk.Core.Interfaces.Services;
using Kinopoisk.MVC.Models;
using Microsoft.Extensions.Logging;

namespace Kinopoisk.Services.Services;

public class RatingService : BaseService<Rating, RatingDTO, DataTablesRequestModel>, IRatingService
{
    private readonly IRatingRepository _repository;
    private readonly IUnitOfWork _uow;
    public RatingService(IUnitOfWork uow,
                         IMapper mapper,
                         IRatingRepository repository,
                         ILogger<RatingService> logger) : base(uow, mapper, logger)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task<Result<double>> CalculateSitesRating(int filmId)
    {
        var result = await _repository.CalculateSitesRating(filmId);
        if (result.IsFailure)
            return Result.Failure<double>(result.Error);

        await _uow.SaveChangesAsync();
        return Result.Success(result.Value);
    }

    public async Task<Result<double>> GetUserRating(int filmId, int userId)
    {
        var result = await _repository.GetUserRating(filmId, userId);

        if (result.IsFailure)
            return Result.Failure<double>(result.Error);

        return Result.Success(result.Value);
    }
}
