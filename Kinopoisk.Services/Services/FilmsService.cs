using AutoMapper;
using CSharpFunctionalExtensions;
using Kinopoisk.Core.Enitites;
using Kinopoisk.DataAccess.Interfaces;
using Kinopoisk.Services.Interfaces;

namespace Kinopoisk.Services.Services;

public class FilmsService : IFilmsService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public FilmsService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Film>> GetAllAsync()
    {
        var films = await _uow.FilmRepository.GetAllAsync();
        var filsmDtos = _mapper.Map<IEnumerable<Film>>(films);
        return filsmDtos;
    }

    public async Task<Result<Film>> GetByIdAsync(int? id)
    {
        if (id == null)
            return Result.Failure<Film>("Id is null");
        
        var film = await _uow.FilmRepository.GetByIdAsync(id.Value);
        var filmDto = _mapper.Map<Film>(film);
        return filmDto == null
            ? Result.Failure<Film>("Film not found")
            : Result.Success(filmDto);
    }

    public async Task<Result<Film>> AddAsync(Film entity)
    {
        if (entity == null)
            return Result.Failure<Film>("Entity is null");

        var result = await _uow.FilmRepository.AddAsync(entity);
        if (result.IsSuccess)
        {
            await _uow.SaveChangesAsync();
            return Result.Success(entity);
        }
        return Result.Failure<Film>(result.Error);
    }

    public async Task<Result<Film>> UpdateAsync(Film entity)
    {
        if (entity == null)
            return Result.Failure<Film>("Entity is null");

        var result = await _uow.FilmRepository.UpdateAsync(entity);
        if (result.IsSuccess)
        {
            await _uow.SaveChangesAsync();
            return Result.Success(entity);
        }
        return Result.Failure<Film>(result.Error);
    }

    public async Task<Result> DeleteAsync(int? id)
    {
        if (id == null)
            return Result.Failure("Id is null");

        var result = await _uow.FilmRepository.DeleteAsync(id.Value);
        if (result.IsSuccess)
        {
            await _uow.SaveChangesAsync();
            return Result.Success();
        }
        return Result.Failure(result.Error);
    }
}
