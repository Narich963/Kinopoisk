using AutoMapper;
using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Interfaces.Repositories;
using Kinopoisk.Core.Interfaces.Services;

namespace Kinopoisk.Services.Services;

public class OmdbService : IOmdbService
{
    private readonly IOmdbRepository _repository;
    private readonly IMapper _mapper;

    public OmdbService(IOmdbRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<FilmDTO>> ImportFilm(string idOrTitle)
    {
        var result = await _repository.ImportFilm(idOrTitle);
        if (result.IsFailure)
            return Result.Failure<FilmDTO>(result.Error);

        var filmDto = _mapper.Map<FilmDTO>(result.Value);

        return Result.Success(filmDto);
    }
}
