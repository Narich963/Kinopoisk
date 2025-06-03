using AutoMapper;
using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Filters;
using Kinopoisk.Core.Interfaces.Repositories;
using Kinopoisk.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Kinopoisk.Services.Services;

public class FilmService : BaseService<Film, FilmDTO, FilmFilter>, IFilmService
{
    private readonly IMapper _mapper;
    private readonly IFilmRepository _repository;
    private readonly IUnitOfWork _uow;
    private readonly ILogger<FilmService> _logger;

    public FilmService(IUnitOfWork uow, IMapper mapper, IFilmRepository repository, ILogger<FilmService> logger) : base(uow, mapper, logger)
    {
        _mapper = mapper;
        _repository = repository;
        _uow = uow;
        _logger = logger;
    }

    #region Get Methods
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
    #endregion

    #region CRUD Methods
    public async Task<Result<FilmDTO>> AddOrEditAsync(FilmDTO filmDto, bool? isNew)
    {
        if (!isNew.HasValue)
        {
            _logger.Log(LogLevel.Error, "isNew parameter is null in AddOrEditAsync method");
            return Result.Failure<FilmDTO>("isNew parameter is null");
        }
        
        var directorId = filmDto.Employees.FirstOrDefault(e => e.IsDirector)?.FilmEmployeeId;
        filmDto.Employees.Clear();

        var filmResult = isNew.Value
            ? await _repository.AddAsync(_mapper.Map<Film>(filmDto))
            : await _repository.UpdateAsync(_mapper.Map<Film>(filmDto));

        if (filmResult.IsFailure)
        {
            _logger.Log(LogLevel.Error, "An error occurred while trying to {Action} Film. Message: {Error}", isNew.Value ? "add" : "update", filmResult.Error);
            return Result.Failure<FilmDTO>(filmResult.Error);
        }

        var employesResult = await UpdateFilmEmployees(filmResult.Value.Id, directorId, filmDto.SelectedActorIds);
        if (employesResult.IsFailure)
        {
            _logger.Log(LogLevel.Error, "An error occurred while trying to update film employees. Message: {Error}", employesResult.Error);
            return Result.Failure<FilmDTO>(employesResult.Error);
        }

        var genresResult = await UpdateFilmGenres(filmResult.Value.Id, filmDto.SelectedGenreIds);
        if (genresResult.IsFailure)
        {
            _logger.Log(LogLevel.Error, "An error occurred while trying to update film genres. Message: {Error}", genresResult.Error);
            return Result.Failure<FilmDTO>(genresResult.Error);
        }
        _logger.Log(LogLevel.Information, "Film {Action} successfully", isNew.Value ? "added" : "updated");
        return Result.Success(_mapper.Map<FilmDTO>(filmResult.Value));
    }
    public async Task<Result> UpdateFilmGenres(int? filmId, List<int> genreIds)
    {
        if (!filmId.HasValue)
            return Result.Failure("Film ID or Genre ID is null");

        var genresResult = await _repository.UpdateGenres(filmId.Value, genreIds);
        if (genresResult.IsFailure)
            return Result.Failure(genresResult.Error);

        await _uow.SaveChangesAsync();  
        return Result.Success();
    }

    public async Task<Result> UpdateFilmEmployees(int? filmId, int? directorId, List<int> actorIds)
    {
        if (!filmId.HasValue || !directorId.HasValue)
            return Result.Failure("Film ID is null");

        var result = await _repository.UpdateFilmEmployees(filmId.Value, directorId.Value, actorIds);

        if (result.IsFailure)
            return Result.Failure(result.Error);

        await _uow.SaveChangesAsync();
        return Result.Success();
    }
    #endregion
}
