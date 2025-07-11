﻿using CSharpFunctionalExtensions;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Filters;
using Kinopoisk.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Globalization;
using Kinopoisk.DataAccess.Extensions;
using Kinopoisk.Core.Enums;

namespace Kinopoisk.DataAccess.Repositories;

public class FilmRepository : GenericRepository<Film, FilmFilter>, IFilmRepository
{
    private readonly KinopoiskContext _context;

    public FilmRepository(KinopoiskContext context) : base(context)
    {
        _context = context;
    }

    #region Get Methods
    public async Task<DataTablesResult<Film>> GetPagedAsync(FilmFilter filter, IQueryable<Film> query = null)
    {
        query = _context.Films;

        query = Filter(filter, query);
        query = Search(filter, query);
        query = Order(filter, query);

        return await base.GetPagedAsync(filter, query);
    }

    public async Task<Result<Film>> GetByIdAsync(int id)
    {
        var query = _context.Films;

        var filmResult = await base.GetByIdAsync(id, query);

        return filmResult.IsSuccess
            ? Result.Success(filmResult.Value)
            : Result.Failure<Film>("Film not found");
    }
    public async Task<IEnumerable<Film>> GetAllAsync()
    {
        var films = await _context.Films
            .ToListAsync();
        return films;
    }
    #endregion

    #region CRUD Methods
    public new async Task<Result<Film>> AddAsync(Film film)
    {
        if (film == null)
            throw new ArgumentNullException(nameof(film), "Film cannot be null");

        await _context.Films.AddAsync(film);
        await _context.SaveChangesAsync(); // It's necessary to save changes here to get the ID of the newly added film
        return Result.Success(film);
    }
    public async Task<Result> UpdateGenres(int filmId, List<int> genreIds)
    {
        if (!genreIds.Any())
            return Result.Success();

        var existingGenres = await _context.FilmGenres
            .Where(fg => fg.FilmId == filmId)
            .ToListAsync();

        var genresToRemove = existingGenres
            .Where(fg => !genreIds.Contains(fg.GenreId))
            .ToList();

        if (genresToRemove.Any())
            _context.FilmGenres.RemoveRange(genresToRemove);

        var genresToAdd = genreIds
            .Where(id => !existingGenres.Any(fg => fg.GenreId == id))
            .Select(id => new FilmGenre
            {
                FilmId = filmId,
                GenreId = id
            })
            .ToList();

        if (genresToAdd.Any())
            await _context.FilmGenres.AddRangeAsync(genresToAdd);

        return Result.Success();
    }
    public async Task<Result> UpdateFilmEmployees(int filmId, int directorId, List<int> actorIds)
    {
        var director = await _context.FilmEmployeeRoles
            .FirstOrDefaultAsync(f => f.FilmId == filmId && f.IsDirector);

        if (director?.FilmEmployeeID != directorId)
        {
            if (director != null)
                _context.FilmEmployeeRoles.Remove(director);

            var existing = await _context.FilmEmployeeRoles
                .FirstOrDefaultAsync(f => f.FilmId == filmId && f.FilmEmployeeID == directorId);

            if (existing != null)
                _context.Entry(existing).State = EntityState.Detached;

            director = new FilmEmployeeRole
            {
                FilmId = filmId,
                FilmEmployeeID = directorId,
                IsDirector = true,
                Role = 0
            };
            await _context.FilmEmployeeRoles.AddAsync(director);
        }

        var actors = await _context.FilmEmployeeRoles
            .Where(f => f.FilmId == filmId && !f.IsDirector)
            .ToListAsync();

        var actorsToRemove = actors
            .Where(a => !actorIds.Contains(a.FilmEmployeeID))
            .ToList();

        var actorsToAdd = actorIds
            .Where(id => !actors.Any(a => a.FilmEmployeeID == id))
            .Select(id => new { Id = id, Index = actorIds.IndexOf(id) })
            .Select(x => new FilmEmployeeRole
            {
                FilmId = filmId,
                FilmEmployeeID = x.Id,
                IsDirector = false,
                Role = x.Index
            })
            .ToList();

        if (actorsToRemove.Count > 0)
            _context.FilmEmployeeRoles.RemoveRange(actorsToRemove);

        if (actorsToAdd.Count > 0)
            await _context.FilmEmployeeRoles.AddRangeAsync(actorsToAdd);

        var actorsToUpdate = actors
            .Where(a => actorIds.Contains(a.FilmEmployeeID)
                && a.Role != actorIds.IndexOf(a.FilmEmployeeID))
            .ToList();

        if (actorsToUpdate.Count == 0 && actorsToAdd.Count == 0)
            return Result.Success();

        actorsToUpdate.ForEach(a =>
        {
            a.Role = actorIds.IndexOf(a.FilmEmployeeID);
        });

        _context.FilmEmployeeRoles.UpdateRange(actorsToUpdate);

        return Result.Success();
    }
    #endregion

    #region Filter and Order Methods
    public IQueryable<Film> Filter(FilmFilter filter, IQueryable<Film> query)
    {
        // By name
        if (!string.IsNullOrEmpty(filter.Name))
            query = query.Where(q => q.GetLocalizationValue(PropertyEnum.Name, CultureInfo.CurrentCulture.TwoLetterISOLanguageName).ToLower().Contains(filter.Name));

        // By year
        if (!string.IsNullOrEmpty(filter.Year))
            query = query.Where(q => q.PublishDate.Value.Year.ToString().Contains(filter.Year));

        // By country
        if (!string.IsNullOrEmpty(filter.Country))
            query = query.Where(q => q.Country.GetLocalizationValue(PropertyEnum.Name, CultureInfo.CurrentCulture.TwoLetterISOLanguageName).ToLower().Contains(filter.Country));

        // By actors
        if (!string.IsNullOrEmpty(filter.Actor))
            query = query.Where(q => q.Employees
                .Any(e => !e.IsDirector && e.FilmEmployee.GetLocalizationValue(PropertyEnum.Name, CultureInfo.CurrentCulture.TwoLetterISOLanguageName).ToLower().Contains(filter.Name)));

        // By director
        if (!string.IsNullOrEmpty(filter.Director))
            query = query.Where(q => q.Employees
                .Any(e => e.IsDirector && e.FilmEmployee.GetLocalizationValue(PropertyEnum.Name, CultureInfo.CurrentCulture.TwoLetterISOLanguageName).ToLower().Contains(filter.Name)));

        return query;
    }
    public IQueryable<Film> Search(FilmFilter filter, IQueryable<Film> query)
    {
        if (!string.IsNullOrEmpty(filter.Search?.Value))
        {
            string searchValue = filter.Search.Value.ToLower();
            query = query.Where(f => f.GetLocalizationValue(PropertyEnum.Name, CultureInfo.CurrentCulture.TwoLetterISOLanguageName).ToLower().Contains(searchValue) ||
                                     f.PublishDate.Value.Year.ToString().Contains(searchValue) ||
                                     f.Country.GetLocalizationValue(PropertyEnum.Name, CultureInfo.CurrentCulture.TwoLetterISOLanguageName).ToLower().Contains(searchValue) ||
                                     f.Employees.Any(e => e.FilmEmployee.GetLocalizationValue(PropertyEnum.Name, CultureInfo.CurrentCulture.TwoLetterISOLanguageName).ToLower().Contains(searchValue)));
        }

        return query;
    }
    public IQueryable<Film> Order(FilmFilter filter, IQueryable<Film> query)
    {
        Expression<Func<Film, object>> orderBy = null;

        if (filter.Order != null && filter.Order.Count > 0)
        {
            var order = filter.Order[0];
            var columnName = filter.Columns[order.Column].Data;

            var culture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            switch (columnName)
            {
                case "country":
                case "countryFlagLink":
                    orderBy = f => f.Country.GetLocalizationValue(PropertyEnum.Name, CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
                    break;
                case "imdbRating":
                    orderBy = f => f.IMDBRating;
                    break;
                case "usersRating":
                    orderBy = f => f.Ratings.Average(r => r.Value);
                    break;
                case "director.filmEmployee.name":
                    orderBy = f => f.Employees
                        .Where(e => e.IsDirector)
                        .Select(e => e.FilmEmployee.GetLocalizationValue(PropertyEnum.Name, CultureInfo.CurrentCulture.TwoLetterISOLanguageName))
                        .FirstOrDefault();
                    break;
                case "name":
                    orderBy = f => f.GetLocalizationValue(PropertyEnum.Name, CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
                    break;
                case "description":
                    orderBy = f => f.GetLocalizationValue(PropertyEnum.Description, CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
                    break;
                default:
                    orderBy = f => EF.Property<Film>(f, ToPascaleCase(columnName));
                    break;
            };

            query = order.Dir == "asc"
                ? query.OrderBy(orderBy)
                : query.OrderByDescending(orderBy);
        }

        return query;
    }
    #endregion
}
