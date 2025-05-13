using AutoMapper;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Services.DTO;
using Kinopoisk.WebApi.Contracts;

namespace Kinopoisk.WebApi.Initializers;

public class MapperInitializer : Profile
{
    public MapperInitializer()
    {
        CreateFilmsMap();

        CreateMap<ActorDTO, Actor>().ReverseMap();
        CreateMap<ActorRoleDTO, ActorRole>().ReverseMap();
        CreateMap<DirectorDTO, Director>().ReverseMap();
        CreateMap<GenreDTO, Genre>().ReverseMap();
        CreateMap<UserDTO, User>().ReverseMap();
    }
    private void CreateFilmsMap()
    {
        CreateMap<FilmDTO, Film>().ReverseMap();
        CreateMap<FilmResponse, FilmDTO>().ReverseMap();
        CreateMap<FilmCreateRequest, FilmDTO>().ReverseMap();
    }
}
