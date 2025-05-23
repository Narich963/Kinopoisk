﻿using AutoMapper;
using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Interfaces.Repositories;
using Kinopoisk.Core.Interfaces.Services;
using Kinopoisk.MVC.Models;

namespace Kinopoisk.Services.Services;

public class RatingService : BaseService<Rating, RatingDTO, DataTablesRequestModel>, IRatingService
{
    private readonly IMapper _mapper;
    private readonly IRatingRepository _repository;
    public RatingService(IUnitOfWork uow, IMapper mapper, IRatingRepository repository) : base(uow, mapper)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<Result<double>> GetFilmRating(int filmId)
    {
        var result = await _repository.GetFilmRating(filmId);
        if (result.IsFailure)
            return Result.Failure<double>(result.Error);

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
