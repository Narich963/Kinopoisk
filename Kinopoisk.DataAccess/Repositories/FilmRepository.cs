using CSharpFunctionalExtensions;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Filters;
using Kinopoisk.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Kinopoisk.DataAccess.Repositories;

public class FilmRepository : GenericRepository<Film>, IFilmRepository
{
    private readonly KinopoiskContext _context;

    public FilmRepository(KinopoiskContext context) : base(context)
    {
        _context = context;
    }

    public async Task<PagedResult<Film>> GetPagedAsync(FilterModel<Film> filterModel)
    {
        var query = _context.Films
            .Include(f => f.Genres)
            .Include(f => f.Comments)
                .ThenInclude(c => c.User)
            .Include(f => f.Ratings)
            .Include(f => f.Employees)
                .ThenInclude(a => a.FilmEmployee)
            .Include(f => f.Country)
            .AsQueryable();

        return await GetPagedAsync(filterModel, query);
    }

    public async Task<Result<Film>> GetByIdAsync(int id)
    {
        var query = _context.Films
            .Include(f => f.Genres)
            .Include(f => f.Comments)
                .ThenInclude(c => c.User)
            .Include(f => f.Ratings)
            .Include(f => f.Employees)
                .ThenInclude(a => a.FilmEmployee)
            .Include(f => f.Country);

        var filmResult = await base.GetByIdAsync(id, query);

        return filmResult.IsSuccess
            ? Result.Success(filmResult.Value)
            : Result.Failure<Film>("Film not found");
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

}
