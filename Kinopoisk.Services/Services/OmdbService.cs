using AutoMapper;
using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Interfaces.Repositories;
using Kinopoisk.Core.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Kinopoisk.Services.Services;

public class OmdbService : IOmdbService
{
    private readonly IOmdbRepository _repository;
    private readonly IMapper _mapper;
    private readonly HttpClient _httpClient;
    private const string API_KEY = "2bafad4f";
    private const string API_URL = "https://www.omdbapi.com/";
    private readonly IUnitOfWork _uow;
    private readonly ILogger<OmdbService> _logger;

    public OmdbService(IOmdbRepository repository, IMapper mapper, HttpClient httpClient, IUnitOfWork uow, ILogger<OmdbService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _httpClient = httpClient;
        _uow = uow;
        _logger = logger;
    }

    public async Task<Result<FilmDTO>> ImportFilm(string idOrTitle)
    {
        var urlById = $"{API_URL}?i={Uri.EscapeDataString(idOrTitle)}&apikey={API_KEY}";
        var urlByTitle = $"{API_URL}?t={Uri.EscapeDataString(idOrTitle)}&apikey={API_KEY}";

        var response = await _httpClient.GetAsync(urlByTitle);
        
        var json = await response.Content.ReadAsStringAsync();
        var omdbResponse = JsonSerializer.Deserialize<OmdbResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (omdbResponse == null || string.Equals(omdbResponse.Response, "False", StringComparison.OrdinalIgnoreCase))
        {
            response = await _httpClient.GetAsync(urlById);
            json = await response.Content.ReadAsStringAsync();

            omdbResponse = JsonSerializer.Deserialize<OmdbResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (omdbResponse == null || string.Equals(omdbResponse.Response, "False", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogError("Failed to fetch data from OMDb API for id or title: {IdOrTitle}", idOrTitle);
                return Result.Failure<FilmDTO>("Failed to fetch data from OMDb API.");
            }
        }

        var result = await _repository.ImportFilm(omdbResponse);
        if (result.IsFailure)
        {
            _logger.LogError("Failed to import film from OMDb API: {Error}", result.Error);
            return Result.Failure<FilmDTO>(result.Error);
        }

        var filmDto = _mapper.Map<FilmDTO>(result.Value);

        await _uow.SaveChangesAsync();

        _logger.LogInformation("Film imported successfully from OMDb API: {Title}", filmDto.Name);
        return Result.Success(filmDto);
    }
}
