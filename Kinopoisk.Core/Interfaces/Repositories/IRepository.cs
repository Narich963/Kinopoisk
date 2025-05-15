using CSharpFunctionalExtensions;

namespace Kinopoisk.Core.Interfaces.Repositories;

public interface IRepository<T>
{
    
    Task<Result<T>> AddAsync(T entity);
    Task<Result<T>> UpdateAsync(T entity);
    Task<Result> DeleteAsync(int id);
}
