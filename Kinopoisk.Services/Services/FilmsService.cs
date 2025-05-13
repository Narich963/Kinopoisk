using AutoMapper;
using CSharpFunctionalExtensions;
using Kinopoisk.Core.Enitites;
using Kinopoisk.DataAccess.Interfaces;
using Kinopoisk.Services.DTO;
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

    public async Task<IEnumerable<FilmDTO>> GetAllAsync()
    {
        var films = await _uow.FilmRepository.GetAllAsync();
        var filsmDtos = _mapper.Map<IEnumerable<FilmDTO>>(films);
        return filsmDtos;
    }

    public async Task<Result<FilmDTO>> GetByIdAsync(int? id)
    {
        if (id == null)
            return Result.Failure<FilmDTO>("Id is null");
        
        var film = await _uow.FilmRepository.GetByIdAsync(id.Value);
        var filmDto = _mapper.Map<FilmDTO>(film);
        return filmDto == null
            ? Result.Failure<FilmDTO>("Film not found")
            : Result.Success(filmDto);
    }

    public async Task<Result<FilmDTO>> AddAsync(FilmDTO dto)
    {
        if (dto == null)
            return Result.Failure<FilmDTO>("Entity is null");

        var film = _mapper.Map<Film>(dto);
        var result = await _uow.FilmRepository.AddAsync(film);
        if (result.IsSuccess)
        {
            await _uow.SaveChangesAsync();
            return Result.Success(dto);
        }
        return Result.Failure<FilmDTO>(result.Error);
    }

    public async Task<Result<FilmDTO>> UpdateAsync(FilmDTO dto)
    {
        if (dto == null)
            return Result.Failure<FilmDTO>("Entity is null");

        var film = _mapper.Map<Film>(dto);
        var result = await _uow.FilmRepository.UpdateAsync(film);
        if (result.IsSuccess)
        {
            await _uow.SaveChangesAsync();
            return Result.Success(dto);
        }
        return Result.Failure<FilmDTO>(result.Error);
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
