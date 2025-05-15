using CSharpFunctionalExtensions;
using Kinopoisk.Core.Interfaces.Repositories;
using Kinopoisk.Services.Interfaces;

namespace Kinopoisk.Services.Services;

public abstract class BaseService<T> : IService<T>
{
    protected readonly IRepository<T> _repository;

    protected BaseService(IRepository<T> repository)
    {
        _repository = repository;
    }

    public async Task<Result<T>> AddAsync(T dto)
    {
        if (dto == null)
            return Result.Failure<T>("Dto is null");

        var result = await _repository.AddAsync(dto);

        if (result.IsSuccess)
            return Result.Success(result.Value);

        return Result.Failure<T>(result.Error);
    }

    public async Task<Result<T>> UpdateAsync(T dto)
    {
        if (dto == null)
            return Result.Failure<T>("Dto is null");

        var result = await _repository.UpdateAsync(dto);

        if (result.IsSuccess)
            return Result.Success(result.Value);

        return Result.Failure<T>(result.Error);
    }

    public async Task<Result> DeleteAsync(int? id)
    {
        if (!id.HasValue)
            return Result.Failure("Id is null");

        var result = await _repository.DeleteAsync(id.Value);

        if (result.IsSuccess)
            return Result.Success();

        return Result.Failure(result.Error);
    }
}
