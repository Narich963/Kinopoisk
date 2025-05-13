using AutoMapper;
using Kinopoisk.Core.Enitites;
using Kinopoisk.MVC.Models;
using Kinopoisk.Services.DTO;

namespace Kinopoisk.MVC.Initializers;

public class MapperInitializer : Profile
{
    public MapperInitializer()
    {
        CreateMap<FilmDTO, Film>().ReverseMap();
        CreateMap<FilmsViewModel, FilmDTO>().ReverseMap();
    }
}
