using AutoMapper;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Interfaces.Repositories;
using Kinopoisk.Core.Interfaces.Services;
using Kinopoisk.MVC.Models;

namespace Kinopoisk.Services.Services;

public class FimEmployeeService : BaseService<FilmEmployee, FilmEmployeeDTO, DataTablesRequestModel>, IFilmEmployeeService
{
    public FimEmployeeService(IUnitOfWork uow, IMapper mapper) : base(uow, mapper)
    {
    }
}
