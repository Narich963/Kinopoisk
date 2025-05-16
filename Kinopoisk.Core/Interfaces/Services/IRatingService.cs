using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;
using Kinopoisk.Services.Interfaces;

namespace Kinopoisk.Core.Interfaces.Services;

public interface IRatingService : IService<RatingDTO>
{
    Task<Result<double>> GetFilmRating(int filmId);
    Task<Result<double>> GetUserRating(int filmId, int userId);
}
