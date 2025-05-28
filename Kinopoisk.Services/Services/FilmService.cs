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

    public async Task<Result> RemoveEmployeeFromFilm(int? filmId, int? employeeId)
    {
        if (!filmId.HasValue || !employeeId.HasValue)
            return Result.Failure("Film ID or Employee ID is null");

        var employeeRole = await _repository.RemoveEmployeeFromFilm(filmId.Value, employeeId.Value);
        if (employeeRole.IsFailure)
            return Result.Failure(employeeRole.Error);

        return Result.Success();
    }
    public async Task<Result> AddActorToFilm(int? filmId, int? employeeId)
    {
        if (!filmId.HasValue || !employeeId.HasValue)
            return Result.Failure("Film ID or Action ID is null");

        var result = await _repository.AddActorToFilm(filmId.Value, employeeId.Value);

        if (result.IsFailure)
            return Result.Failure(result.Error);
        return Result.Success();
    }

    public async Task<Result> RemoveGenreFromFilm(int? filmId, int? genreId)
    {
        if (!filmId.HasValue || !genreId.HasValue)
            return Result.Failure("Film ID or Genre ID is null");

        var genre = await _repository.RemoveGenreFromFilm(filmId.Value, genreId.Value);
        if (genre.IsFailure)
            return Result.Failure(genre.Error);

        return Result.Success();
    }

    public async Task<Result> AddGenreToFilm(int? filmId, int? genreId)
    {
        if (!filmId.HasValue || !genreId.HasValue)
            return Result.Failure("Film ID or Genre ID is null");

        var genre = await _repository.AddGenreToFilm(filmId.Value, genreId.Value);

        if (genre.IsFailure)
            return Result.Failure(genre.Error);
        return Result.Success();
    }
}
