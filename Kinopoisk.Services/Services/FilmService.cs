using AutoMapper;
using CSharpFunctionalExtensions;
using Kinopoisk.Core.Enitites;
using Kinopoisk.DataAccess.Interfaces;
using Kinopoisk.Services.DTO;
using Kinopoisk.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kinopoisk.Services.Services;

public class FilmService : IFilmService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public FilmService(IUnitOfWork uow, IMapper mapper)
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

    public async Task<IEnumerable<FilmDTO>> GetFilteredAsync(string? name, int? year, string? country, string? actorName, string? director)
    {
        var filmsQuery = _uow.FilmRepository.GetAllAsQueryable();

        if (name != null)
            filmsQuery = filmsQuery.Where(f => f.Name.ToLower().Contains(name.ToLower()));
        if (year.HasValue)
            filmsQuery = filmsQuery.Where(f => f.PublishDate.Year == year.Value);
        if (country != null)
            filmsQuery = filmsQuery.Where(f => f.Country.ToLower().Contains(country.ToLower()));
        if (actorName != null)
            filmsQuery = filmsQuery.Where(f => f.Employees.Any(a => a.FilmEmployee.Name.ToLower().Contains(actorName.ToLower())));
        if (director != null)
            filmsQuery = filmsQuery.Where(f => f.Employees.Any(e => e.IsDirector && e.FilmEmployee.Name.ToLower().Contains(director.ToLower())));

        var films = await filmsQuery.ToListAsync();

        var filmsDtos = _mapper.Map<List<FilmDTO>>(films);
        return filmsDtos;
    }

    public async Task<Result<FilmDTO>> GetByIdAsync(int? id)
    {
        if (id == null)
            return Result.Failure<FilmDTO>("Id is null");
        
        var film = await _uow.FilmRepository.GetByIdAsync(id.Value);
        var filmDto = _mapper.Map<FilmDTO>(film.Value);
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
