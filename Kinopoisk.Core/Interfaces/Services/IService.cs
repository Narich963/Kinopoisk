using CSharpFunctionalExtensions;
using Kinopoisk.Core.Filters;
using Kinopoisk.MVC.Models;

namespace Kinopoisk.Services.Interfaces;

public interface IService<TDto, TRequest> where TRequest : DataTablesRequestModel
{
    Task<DataTablesResult<TDto>> GetPagedAsync(TRequest request);
    Task<Result<TDto>> AddAsync(TDto dto);
    Task<Result<TDto>> UpdateAsync(TDto dto);
    Task<Result> DeleteAsync(int? id);
}
