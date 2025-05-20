using CSharpFunctionalExtensions;
using Kinopoisk.Core.Filters;
using Kinopoisk.Core.Interfaces.Repositories;
using Kinopoisk.MVC.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Kinopoisk.DataAccess.Repositories;

public class GenericRepository<T> : IRepository<T> where T : class
{
    private readonly KinopoiskContext _context;

    public GenericRepository(KinopoiskContext context)
    {
        _context = context;
    }

    public async Task<Result<T>> GetByIdAsync(int id, IQueryable<T> query)
    {
        var entity = await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        if (entity == null)
            return Result.Failure<T>("Entity not found");

        return Result.Success(entity);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        var entities = await _context.Set<T>().ToListAsync();
        return entities;
    }

    public async Task<DataTablesResult<T>> GetPagedAsync(DataTablesRequestModel model, IQueryable<T> query)
    {
        var data = await query
            .Skip((model.Start / model.Length) * model.Length)
            .Take(model.Length)
            .ToListAsync();

        return new DataTablesResult<T>
        {
            Draw = model.Draw,
            RecordsTotal = await _context.Set<T>().CountAsync(),
            RecordsFiltered = await query.CountAsync(),
            Data = data
        };
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
    protected string ToPascaleCase(string str)
    {
        if (string.IsNullOrEmpty(str))
            return str;
        return char.ToUpper(str[0]) + str.Substring(1);
    }
}
