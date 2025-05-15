using CSharpFunctionalExtensions;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kinopoisk.DataAccess.Repositories;

public class FilmRepository : Repository<Film>, IFilmRepository
{
    private readonly KinopoiskContext _context;

    public FilmRepository(KinopoiskContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Film>> GetAllAsync()
    {
        var films = await _context.Films
            .Include(f => f.Genres)
            .Include(f => f.Comments)
                .ThenInclude(c => c.User)
            .Include(f => f.Ratings)
            .Include(f => f.Employees)
                .ThenInclude(a => a.FilmEmployee)
            .Include(f => f.Country)
            .ToListAsync();
        return films;
    }

    public IQueryable<Film> GetAllAsQueryable()
    {
        var films = _context.Films
            .Include(f => f.Genres)
            .Include(f => f.Comments)
                .ThenInclude(c => c.User)
            .Include(f => f.Ratings)
            .Include(f => f.Employees)
                .ThenInclude(a => a.FilmEmployee)
            .Include(f => f.Country)
            .AsQueryable();
        return films;
    }

    public async Task<Result<Film>> GetByIdAsync(int id)
    {
        var film = await _context.Films
            .Include(f => f.Genres)
            .Include(f => f.Comments)
                .ThenInclude(c => c.User)
            .Include(f => f.Ratings)
            .Include(f => f.Employees)
                .ThenInclude(a => a.FilmEmployee)
            .Include(f => f.Country)
            .FirstOrDefaultAsync(f => f.Id == id);

        return film == null
            ? Result.Failure<Film>("Film not found")
            : Result.Success(film);
    }
}
