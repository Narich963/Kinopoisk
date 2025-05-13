using AutoMapper;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Services.DTO;
using Kinopoisk.WebApi.Contracts;

namespace Kinopoisk.WebApi.Initializers;

public class MapperInitializer : Profile
{
    public MapperInitializer()
    {
        CreateMap<FilmDTO, Film>().ReverseMap();
        CreateMap<FilmResponse, FilmDTO>().ReverseMap();
        CreateMap<FilmCreateRequest, FilmDTO>().ReverseMap();
    }
}
