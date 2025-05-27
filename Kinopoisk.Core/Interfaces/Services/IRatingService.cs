using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;
using Kinopoisk.MVC.Models;
using Kinopoisk.Services.Interfaces;

namespace Kinopoisk.Core.Interfaces.Services;

public interface IRatingService : IService<RatingDTO, DataTablesRequestModel>
{
    Task<Result<double>> CalculateSitesRating(int filmId);
    Task<Result<double>> GetUserRating(int filmId, int userId);
}
