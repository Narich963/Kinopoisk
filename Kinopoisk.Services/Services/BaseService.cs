using AutoMapper;
using CSharpFunctionalExtensions;
using Kinopoisk.Core.Filters;
using Kinopoisk.Core.Interfaces.Repositories;
using Kinopoisk.MVC.Models;
using Kinopoisk.Services.Interfaces;

namespace Kinopoisk.Services.Services;

public abstract class BaseService<TEntity, TDto, TRequest> : IService<TDto, TRequest> 
    where TRequest : DataTablesRequestModel 
    where TEntity : class
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private IRepository<TEntity, TRequest> _repository;

    protected BaseService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
        _repository = _uow.GetRepository<TEntity, TRequest>();
    }

    public async Task<DataTablesResult<TDto>> GetPagedAsync(TRequest request)
    {
        var data = await _repository.GetPagedAsync(request);

        var dataTableResult = new DataTablesResult<TDto>
        {
            Draw = request.Draw,
            RecordsTotal = data.RecordsTotal,
            RecordsFiltered = data.RecordsFiltered,
            Data = _mapper.Map<List<TDto>>(data.Data)
        };

        return dataTableResult;
    }
    public async Task<Result<TDto>> GetByIdAsync(int? id)
    {
        if (!id.HasValue)
            return Result.Failure<TDto>("Id is null");

        var result = await _repository.GetByIdAsync(id.Value);
        if (result.IsSuccess)
        {
            var dto = _mapper.Map<TDto>(result.Value);
            return Result.Success(dto);
        }
        return Result.Failure<TDto>(result.Error);
    }

    public async Task<Result<TDto>> AddAsync(TDto dto)
    {
        if (dto == null)
            return Result.Failure<TDto>("Dto is null");

        var entity = _mapper.Map<TEntity>(dto);

        var result = await _repository.AddAsync(entity);

        if (result.IsSuccess)
        {
            dto = _mapper.Map<TDto>(result.Value);
            return Result.Success(dto);
        }
        return Result.Failure<TDto>(result.Error);
    }

    public async Task<Result<TDto>> UpdateAsync(TDto dto)
    {
        if (dto == null)
            return Result.Failure<TDto>("Dto is null");

        var entity = _mapper.Map<TEntity>(dto);
        var result = await _repository.UpdateAsync(entity);

        if (result.IsSuccess)
        {
            dto = _mapper.Map<TDto>(result.Value);
            return Result.Success(dto);
        }
        return Result.Failure<TDto>(result.Error);
    }

    public virtual async Task<Result> DeleteAsync(int? id)
    {
        if (!id.HasValue)
            return Result.Failure("Id is null");

        var result = await _repository.DeleteAsync(id.Value);

        if (result.IsSuccess)
            return Result.Success();

        return Result.Failure(result.Error);
    }

}
