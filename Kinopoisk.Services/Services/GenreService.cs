using AutoMapper;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Interfaces.Repositories;
using Kinopoisk.Core.Interfaces.Services;
using Kinopoisk.MVC.Models;
using Kinopoisk.Services.Interfaces;

namespace Kinopoisk.Services.Services;

public class GenreService : BaseService<Genre, GenreDTO, DataTablesRequestModel>, IGenreService
{
    public GenreService(IUnitOfWork uow, IMapper mapper) : base(uow, mapper)
    {
    }
}
