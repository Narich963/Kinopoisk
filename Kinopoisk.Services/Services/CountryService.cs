using AutoMapper;
using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Interfaces.Repositories;
using Kinopoisk.Core.Interfaces.Services;
using Kinopoisk.MVC.Models;
using Microsoft.Extensions.Logging;

namespace Kinopoisk.Services.Services;

public class CountryService : BaseService<Country, CountryDTO, DataTablesRequestModel>, ICountryService
{
    private readonly IUnitOfWork _uow;
    private readonly ILogger<CountryService> _logger;
    private readonly IRepository<Country, DataTablesRequestModel> _repository;
    public CountryService(IUnitOfWork uow, IMapper mapper, ILogger<CountryService> logger) : base(uow, mapper, logger)
    {
        _uow = uow;
        _logger = logger;
        _repository = _uow.GetGenericRepository<Country, DataTablesRequestModel>();
    }
    public async override Task<Result> DeleteAsync(int? id)
    {
        if (!id.HasValue)
        {
            _logger.Log(LogLevel.Error, "Id is null in DeleteAsync method");
            return Result.Failure("Id is null");
        }

        var country = await _repository.GetByIdAsync(id.Value, includes: g => g.Films);

        if (country.IsFailure)
        {
            _logger.Log(LogLevel.Error, "An error occured while trying to delete Country with Id - {Id}. Message: {Error}", id.Value, country.Error);
            return Result.Failure(country.Error);
        }

        if (country.Value.Films.Any())
        {
            _logger.Log(LogLevel.Warning, "Attempt to delete Country with Id - {Id} that has associated films", id.Value);
            return Result.Failure("You can't delete country that has films");
        }

        return await base.DeleteAsync(id);
    }
}
