using CSharpFunctionalExtensions;

namespace Kinopoisk.Services.Interfaces;

public interface IService<T>
{
    Task<Result<T>> AddAsync(T dto);
    Task<Result<T>> UpdateAsync(T dto);
    Task<Result> DeleteAsync(int? id);
}
