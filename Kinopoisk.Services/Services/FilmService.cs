using AutoMapper;
using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Filters;
using Kinopoisk.Core.Interfaces.Repositories;
using Kinopoisk.MVC.Models;
using Kinopoisk.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Kinopoisk.Services.Services;

public class FilmService : BaseService<Film>, IFilmService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public FilmService(IUnitOfWork uow, IMapper mapper) : base(uow.FilmRepository)
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

    public async Task<DataTablesResult<FilmDTO>> GetPagedAsync(FilmFilter model)
    {

        var pagedResult = await _uow.FilmRepository.GetPagedAsync(model);

        var filmDTOResult = new DataTablesResult<FilmDTO>
        {
            Draw = pagedResult.Draw,
            RecordsTotal = pagedResult.RecordsTotal,
            RecordsFiltered = pagedResult.RecordsFiltered,
            Data = _mapper.Map<List<FilmDTO>>(pagedResult.Data)
        };

        return filmDTOResult;
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
