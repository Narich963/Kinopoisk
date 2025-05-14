using CSharpFunctionalExtensions;
using Kinopoisk.Core.Enitites;
using Kinopoisk.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kinopoisk.DataAccess.Repositories;

public class FilmsRepository : IRepository<Film>
{
    private readonly KinopoiskContext _context;

    public FilmsRepository(KinopoiskContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Film>> GetAllAsync()
    {
        var films = await _context.Films
            .Include(f => f.Genres)
            .Include(f => f.Comments)
            .Include(f => f.Ratings)
            .Include(f => f.ActorRoles)
                .ThenInclude(a => a.Actor)
            .Include(f => f.Director)
            .ToListAsync();
        return films;
    }

    public IQueryable<Film> GetAllAsQueryable()
    {
        var films = _context.Films
            .Include(f => f.Genres)
            .Include(f => f.Comments)
            .Include(f => f.Ratings)
            .Include(f => f.ActorRoles)
                .ThenInclude(a => a.Actor)
            .Include(f => f.Director)
            .AsQueryable();
        return films;
    }

    public async Task<Result<Film>> GetByIdAsync(int id)
    {
        var film = await _context.Films
            .Include(f => f.Genres)
            .Include(f => f.Comments)
            .Include(f => f.Ratings)
            .Include(f => f.ActorRoles)
                .ThenInclude(a => a.Actor)
            .Include(f => f.Director)
            .FirstOrDefaultAsync(f => f.Id == id);

        return film == null
            ? Result.Failure<Film>("Film not found")
            : Result.Success(film);
    }

    public async Task<Result<Film>> AddAsync(Film entity)
    {
        var result = await _context.Films.AddAsync(entity);
        if (result.State == EntityState.Added)
        {
            await _context.SaveChangesAsync();
            return Result.Success(entity);
        }
        return Result.Failure<Film>("Failed to add film");
    }

    public async Task<Result<Film>> UpdateAsync(Film entity)
    {
        var result = _context.Films.Update(entity);
        if (result.State == EntityState.Modified)
        {
            await _context.SaveChangesAsync();
            return Result.Success(entity);
        }
        return Result.Failure<Film>("Failed to update film");
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var film = await _context.Films.FindAsync(id);
        if (film == null)
            return Result.Failure("Film not found");

        _context.Remove(film);
        await _context.SaveChangesAsync();
        return Result.Success();
    }
}
