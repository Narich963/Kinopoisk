using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Filters;
using Kinopoisk.MVC.Models;
using System.Linq.Expressions;

namespace Kinopoisk.Core.Interfaces.Repositories;

public interface IRepository<T>
{
    Task<Result<T>> GetByIdAsync(int id, IQueryable<T> query);
    Task<IEnumerable<T>> GetAllAsync();
    Task<DataTablesResult<T>> GetPagedAsync(DataTablesRequestModel model, IQueryable<T> query);
    Task<Result<T>> AddAsync(T entity);
    Task<Result<T>> UpdateAsync(T entity);
    Task<Result> DeleteAsync(int id);
}
