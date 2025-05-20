using CSharpFunctionalExtensions;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Interfaces.Repositories;
using Kinopoisk.MVC.Models;
using Microsoft.EntityFrameworkCore;

namespace Kinopoisk.DataAccess.Repositories;

public class RatingRepository : GenericRepository<Rating, DataTablesRequestModel>, IRatingRepository
{
    private readonly KinopoiskContext _context;
    public RatingRepository(KinopoiskContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Result<double>> GetFilmRating(int filmId)
    {
        var film = await _context.Films
            .Include(f => f.Ratings)
            .FirstOrDefaultAsync(f => f.Id == filmId);

        if (film == null)
            return Result.Failure<double>("Film not found");

        var rating = Math.Round(film.Ratings.Sum(r => r.Value) / film.Ratings.Count, 1);
        return Result.Success(rating);
    }

    public async Task<Result<double>> GetUserRating(int filmId, int userId)
    {
        var film = await _context.Films
            .Include(f => f.Ratings)
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == filmId);

        if (film == null)
            return Result.Failure<double>("Film not found");

        var userRating = film.Ratings.FirstOrDefault(r => r.UserId == userId);

        return userRating == null
            ? Result.Success(0.0)
            : Result.Success(userRating.Value);
    }
}
