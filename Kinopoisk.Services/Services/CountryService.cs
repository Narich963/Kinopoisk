using AutoMapper;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Interfaces.Repositories;
using Kinopoisk.Core.Interfaces.Services;
using Kinopoisk.MVC.Models;

namespace Kinopoisk.Services.Services;

public class CountryService : BaseService<Country, CountryDTO, DataTablesRequestModel>, ICountryService
{
    public CountryService(IUnitOfWork uow, IMapper mapper) : base(uow, mapper)
    {
    }
}
