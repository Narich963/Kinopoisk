using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Filters;
using System.Linq.Expressions;

namespace Kinopoisk.Core.Interfaces.Repositories;

public interface IRepository<T>
{
    Task<Result<T>> GetByIdAsync(int id, IQueryable<T> query);
    Task<IEnumerable<T>> GetAllAsync();
    Task<PagedResult<T>> GetPagedAsync(FilterModel<T> filter, IQueryable<T> query);
    Task<Result<T>> AddAsync(T entity);
    Task<Result<T>> UpdateAsync(T entity);
    Task<Result> DeleteAsync(int id);
}
