using AutoMapper;
using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Interfaces.Repositories;
using Kinopoisk.Core.Interfaces.Services;
using Kinopoisk.DataAccess;
using System.Text.Json;

namespace Kinopoisk.Services.Services;

public class OmdbService : IOmdbService
{
    private readonly IOmdbRepository _repository;
    private readonly IMapper _mapper;
    private readonly HttpClient _httpClient;
    private const string API_KEY = "2bafad4f";
    private const string API_URL = "https://www.omdbapi.com/";

    public OmdbService(IOmdbRepository repository, IMapper mapper, HttpClient httpClient)
    {
        _repository = repository;
        _mapper = mapper;
        _httpClient = httpClient;
    }

    public async Task<Result<FilmDTO>> ImportFilm(string idOrTitle)
    {
        var urlById = $"{API_URL}?i={Uri.EscapeDataString(idOrTitle)}&apikey={API_KEY}";
        var urlByTitle = $"{API_URL}?t={Uri.EscapeDataString(idOrTitle)}&apikey={API_KEY}";

        var response = await _httpClient.GetAsync(urlByTitle);

        if (!response.IsSuccessStatusCode)
            return Result.Failure<FilmDTO>($"Failed to fetch data from OMDb API. Status code: {response.StatusCode}");
        
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
                return Result.Failure<FilmDTO>("Failed to fetch data from OMDb API.");
            }
        }

        var result = await _repository.ImportFilm(omdbResponse);
        if (result.IsFailure)
            return Result.Failure<FilmDTO>(result.Error);

        var filmDto = _mapper.Map<FilmDTO>(result.Value);

        return Result.Success(filmDto);
    }
}
