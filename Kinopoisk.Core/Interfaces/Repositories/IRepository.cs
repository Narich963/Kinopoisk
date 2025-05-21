using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Filters;
using Kinopoisk.MVC.Models;
using System.Linq.Expressions;

namespace Kinopoisk.Core.Interfaces.Repositories;

public interface IRepository<T, TRequest>
    where TRequest : DataTablesRequestModel
{
    Task<Result<T>> GetByIdAsync(int id, IQueryable<T> query = null);
    Task<IEnumerable<T>> GetAllAsync();
    Task<DataTablesResult<T>> GetPagedAsync(TRequest request, IQueryable<T> query = null);
    Task<Result<T>> AddAsync(T entity);
    Task<Result<T>> UpdateAsync(T entity);
    Task<Result> DeleteAsync(int id);
}
