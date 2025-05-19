using CSharpFunctionalExtensions;
using Kinopoisk.Core.Filters;
using Kinopoisk.Core.Interfaces.Repositories;
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

    public async Task<PagedResult<T>> GetPagedAsync(FilterModel<T> filter, IQueryable<T> query)
    {

        if (filter.SortField != null)
        {
            if (filter.IsAscending)
                query = query.OrderBy(e => EF.Property<T>(e, filter.SortField));
            else
                query = query.OrderByDescending(e => EF.Property<T>(e, filter.SortField));

        }

        if (filter.Predicates != null)
        {
            foreach (var predicate in filter.Predicates)
            {
                query = query.Where(predicate).AsQueryable();
            }
        }            

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();
        var totalPages = (int)Math.Ceiling((double)totalCount / filter.PageSize);

        var result = new PagedResult<T>
        {
            Items = items,
            TotalCount = totalCount,
            TotalPages = totalPages,
            PageSize = filter.PageSize,
            CurrentPage = filter.Page
        };
        return result;
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
