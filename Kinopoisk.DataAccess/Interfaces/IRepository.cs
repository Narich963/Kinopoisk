using CSharpFunctionalExtensions;

namespace Kinopoisk.DataAccess.Interfaces;

public interface IRepository<T>
{
    Task<Result<T>> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    IQueryable<T> GetAllAsQueryable();
    Task<Result<T>> AddAsync(T entity);
    Task<Result<T>> UpdateAsync(T entity);
    Task<Result> DeleteAsync(int id);
}
