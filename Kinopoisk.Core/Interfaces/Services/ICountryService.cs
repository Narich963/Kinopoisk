using Kinopoisk.Core.DTO;
using Kinopoisk.MVC.Models;
using Kinopoisk.Services.Interfaces;

namespace Kinopoisk.Core.Interfaces.Services;

public interface ICountryService : IService<CountryDTO, DataTablesRequestModel>
{

}
