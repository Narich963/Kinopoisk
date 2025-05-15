using CSharpFunctionalExtensions;
using Kinopoisk.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kinopoisk.DataAccess.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly KinopoiskContext _context;

    public Repository(KinopoiskContext context)
    {
        _context = context;
    }

    public async Task<Result<T>> AddAsync(T entity)
    {
        if (entity == null)
            return Result.Failure<T>("Entity is null");

        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();
        return Result.Success(entity);
    }

    public async Task<Result<T>> UpdateAsync(T entity)
    {
        if (entity == null)
            return Result.Failure<T>("Entity is null");

        _context.Update(entity);
        await _context.SaveChangesAsync();
        return Result.Success(entity);
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var entity = _context.Set<T>().Find(id);
        if (entity == null)
            return Result.Failure("Entity not found");

        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

}
