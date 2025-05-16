using AutoMapper;
using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Interfaces.Repositories;
using Kinopoisk.Core.Interfaces.Services;

namespace Kinopoisk.Services.Services;

public class RatingService : BaseService<Rating>, IRatingService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    public RatingService(IUnitOfWork uow, IMapper mapper) : base(uow.RatingRepository)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Result<double>> GetFilmRating(int filmId)
    {
        var result = await _uow.RatingRepository.GetFilmRating(filmId);
        if (result.IsFailure)
            return Result.Failure<double>(result.Error);

        return Result.Success(result.Value);
    }

    public async Task<Result<double>> GetUserRating(int filmId, int userId)
    {
        var result = await _uow.RatingRepository.GetUserRating(filmId, userId);

        if (result.IsFailure)
            return Result.Failure<double>(result.Error);

        return Result.Success(result.Value);
    }

    public async Task<Result<RatingDTO>> AddAsync(RatingDTO dto)
    {
        if (dto == null)
            return Result.Failure<RatingDTO>("Dto is null");

        var rating = _mapper.Map<Rating>(dto);

        var result = await _uow.RatingRepository.AddAsync(rating);
        if (result.IsFailure)
            return Result.Failure<RatingDTO>(result.Error);
        return Result.Success(_mapper.Map<RatingDTO>(result.Value));
    }

    public async Task<Result<RatingDTO>> UpdateAsync(RatingDTO dto)
    {
        if (dto == null)
            return Result.Failure<RatingDTO>("Dto is null");

        var rating = _mapper.Map<Rating>(dto);

        var result = await _uow.RatingRepository.UpdateAsync(rating);
        if (result.IsFailure)
            return Result.Failure<RatingDTO>(result.Error);

        return Result.Success(_mapper.Map<RatingDTO>(result.Value));
    }
}
