using CSharpFunctionalExtensions;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Filters;
using Kinopoisk.Core.Interfaces.Repositories;
using Kinopoisk.MVC.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Kinopoisk.DataAccess.Repositories;

public class FilmRepository : GenericRepository<Film, FilmFilter>, IFilmRepository
{
    private readonly KinopoiskContext _context;

    public FilmRepository(KinopoiskContext context) : base(context)
    {
        _context = context;
    }

    public async Task<DataTablesResult<Film>> GetPagedAsync(FilmFilter filter, IQueryable<Film> query = null)
    {
        query = _context.Films
            .Include(f => f.Genres)
                .ThenInclude(g => g.Genre)
            .Include(f => f.Ratings)
            .Include(f => f.Employees)
                .ThenInclude(a => a.FilmEmployee)
            .Include(f => f.Country)
            .AsQueryable();

        if (!string.IsNullOrEmpty(filter.Name))
            query = query.Where(q => q.Name.ToLower().Contains(filter.Name));

        if (!string.IsNullOrEmpty(filter.Year))
            query = query.Where(q => q.PublishDate.Year.ToString().Contains(filter.Year));

        if (!string.IsNullOrEmpty(filter.Country))
            query = query.Where(q => q.Country.Name.ToLower().Contains(filter.Country.ToLower()));

        if (!string.IsNullOrEmpty(filter.Actor))
            query = query.Where(q => q.Employees
                .Any(e => !e.IsDirector && e.FilmEmployee.Name.ToLower().Contains(filter.Actor.ToLower())));

        if (!string.IsNullOrEmpty(filter.Director))
            query = query.Where(q => q.Employees
                .Any(e => e.IsDirector && e.FilmEmployee.Name.ToLower().Contains(filter.Director.ToLower())));


        if (!string.IsNullOrEmpty(filter.Search?.Value))
        {
            string searchValue = filter.Search.Value.ToLower();
            query = query.Where(f => f.Name.ToLower().Contains(searchValue) ||
                                     f.PublishDate.Year.ToString().Contains(searchValue) ||
                                     f.Country.Name.ToLower().Contains(searchValue) ||
                                     f.Employees.Any(e => e.FilmEmployee.Name.ToLower().Contains(searchValue)));
        }

        Expression<Func<Film, object>> orderBy = null;

        if (filter.Order != null && filter.Order.Count > 0)
        {
            var order = filter.Order[0];
            var columnName = filter.Columns[order.Column].Data;
            switch (columnName)
            {
                case "country":
                case "countryFlagLink":
                    orderBy = f => f.Country.Name;
                    break;
                case "imdbRating":
                    orderBy = f => f.IMDBRating;
                    break;
                case "usersRating":
                    orderBy = f => f.Ratings.Average(r => r.Value);
                    break;
                case "directorName":
                    orderBy = f => f.Employees
                        .Where(e => e.IsDirector)
                        .Select(e => e.FilmEmployee.Name)
                        .FirstOrDefault();
                    break;
                default:
                    orderBy = f => EF.Property<Film>(f, ToPascaleCase(columnName));
                    break;
            };

            query = order.Dir == "asc" 
                ? query.OrderBy(orderBy) 
                : query.OrderByDescending(orderBy);
        }

        return await base.GetPagedAsync(filter, query);
    }

    public async Task<Result<Film>> GetByIdAsync(int id)
    {
        var query = _context.Films
            .Include(f => f.Genres)
                .ThenInclude(g => g.Genre)
            .Include(f => f.Comments)
                .ThenInclude(c => c.User)
            .Include(f => f.Employees)
                .ThenInclude(a => a.FilmEmployee)
            .Include(f => f.Country)
            .AsNoTracking();

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
