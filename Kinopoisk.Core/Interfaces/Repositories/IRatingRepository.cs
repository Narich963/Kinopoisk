using CSharpFunctionalExtensions;
using Kinopoisk.Core.Enitites;
using Kinopoisk.MVC.Models;

namespace Kinopoisk.Core.Interfaces.Repositories;

public interface IRatingRepository : IRepository<Rating, DataTablesRequestModel>
{
    Task<Result<double>> CalculateSitesRating(int filmId);
    Task<Result<double>> GetUserRating(int filmId, int userId);
}
