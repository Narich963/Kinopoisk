﻿using CSharpFunctionalExtensions;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Filters;
using Kinopoisk.Core.Interfaces.Repositories;
using Kinopoisk.MVC.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Kinopoisk.DataAccess.Repositories;

public class GenericRepository<T, TRequest> : IRepository<T, TRequest> 
    where T : class
    where TRequest : DataTablesRequestModel
{
    private readonly KinopoiskContext _context;

    public GenericRepository(KinopoiskContext context)
    {
        _context = context;
    }

    #region Get Methods
    public async Task<Result<T>> GetByIdAsync(int id, IQueryable<T> query = null, params Expression<Func<T, object>>[] includes)
    {
        if (query == null)
            query = _context.Set<T>().AsQueryable();

        if (includes != null && includes.Length > 0)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

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

    public async Task<DataTablesResult<T>> GetPagedAsync(TRequest model, IQueryable<T> query = null)
    {
        if (query == null)
        {
            query = _context.Set<T>().AsQueryable();
            query = SearchByName(model, query);
            query = Order(model, query);
        }

        List<T> data = null;

        if (model.Length == 0)
            data = await query.ToListAsync();
        else
            data = await Paginate(model, query, data);

        return new DataTablesResult<T>
        {
            Draw = model.Draw,
            RecordsTotal = await _context.Set<T>().CountAsync(),
            RecordsFiltered = await query.CountAsync(),
            Data = data
        };
    }
    #endregion

    #region CRUD Methods
    public async Task<Result<T>> AddAsync(T entity)
    {
        if (entity == null)
            return Result.Failure<T>("Entity is null");

        await _context.AddAsync(entity);
        return Result.Success(entity);
    }

    public async Task<Result<T>> UpdateAsync(T entity)
    {
        if (entity == null)
            return Result.Failure<T>("Entity is null");

        _context.Update(entity);
        return Result.Success(entity);
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var entity = _context.Set<T>().Find(id);
        if (entity == null)
            return Result.Failure("Entity not found");

        _context.Set<T>().Remove(entity);
        return Result.Success();
    }
    #endregion

    #region Filter, Sort and Paginate
    private static async Task<List<T>> Paginate(TRequest model, IQueryable<T> query, List<T> data)
    {
        data = await query
            .Skip((model.Start / model.Length) * model.Length)
            .Take(model.Length)
            .ToListAsync();
        return data;
    }

    private IQueryable<T> Order(TRequest model, IQueryable<T> query)
    {
        if (model.Order != null && model.Order.Count > 0)
        {
            var order = model.Order[0];
            string columnName = ToPascaleCase(model.Columns[order.Column].Data);
            bool isDescending = order.Dir.ToLower() == "desc";
            query = isDescending
                ? query.OrderByDescending(e => EF.Property<object>(e, ToPascaleCase(columnName)))
                : query.OrderBy(e => EF.Property<object>(e, ToPascaleCase(columnName)));
        }

        return query;
    }

    private static IQueryable<T> SearchByName(TRequest model, IQueryable<T> query)
    {
        if (model.Search != null && !string.IsNullOrEmpty(model.Search.Value))
        {
            string searchValue = model.Search.Value.ToLower();
            query = query.Where(e => EF.Property<string>(e, "Name").ToLower().Contains(searchValue));
        }

        return query;
    }
    #endregion

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    protected string ToPascaleCase(string str)
    {
        if (string.IsNullOrEmpty(str))
            return str;
        return char.ToUpper(str[0]) + str.Substring(1);
    }
}
