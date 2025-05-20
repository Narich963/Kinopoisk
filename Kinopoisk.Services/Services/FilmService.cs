using AutoMapper;
using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Filters;
using Kinopoisk.Core.Interfaces.Repositories;
using Kinopoisk.Services.Interfaces;

namespace Kinopoisk.Services.Services;

public class FilmService : BaseService<Film, FilmDTO, FilmFilter>, IFilmService
{
    private readonly IMapper _mapper;
    private IFilmRepository _repository;

    public FilmService(IUnitOfWork uow, IMapper mapper, IFilmRepository repository) : base(uow, mapper)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<IEnumerable<FilmDTO>> GetAllAsync()
    {
        var films = await _repository.GetAllAsync();
        var filsmDtos = _mapper.Map<IEnumerable<FilmDTO>>(films);
        return filsmDtos;
    }

    public async Task<Result<FilmDTO>> GetByIdAsync(int? id)
    {
        if (id == null)
            return Result.Failure<FilmDTO>("Id is null");
        
        var film = await _repository.GetByIdAsync(id.Value);
        var filmDto = _mapper.Map<FilmDTO>(film.Value);
        return filmDto == null
            ? Result.Failure<FilmDTO>("Film not found")
            : Result.Success(filmDto);
    }
}
