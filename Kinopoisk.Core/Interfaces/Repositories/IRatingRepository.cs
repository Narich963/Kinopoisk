using CSharpFunctionalExtensions;
using Kinopoisk.Core.Enitites;

namespace Kinopoisk.Core.Interfaces.Repositories;

public interface IRatingRepository : IRepository<Rating>
{
    Task<Result<double>> GetFilmRating(int filmId);
    Task<Result<double>> GetUserRating(int filmId, int userId);
}
