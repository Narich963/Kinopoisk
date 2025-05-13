using AutoMapper;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Services.DTO;

namespace Kinopoisk.MVC.Initializers;

public class MapperInitializer : Profile
{
    public MapperInitializer()
    {
        CreateMap<FilmDTO, Film>().ReverseMap();
    }
}
