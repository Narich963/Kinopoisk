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
        CreateMap<FilmResponse, FilmDTO>()
            .ReverseMap()
            .ForMember(dest => dest.DirectorName, opt => opt.MapFrom(src => src.Director.Name))
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Genres.Select(g => g.Name)))
            .ForMember(dest => dest.ActorRoles, opt => opt.MapFrom(src => src.ActorRoles.OrderBy(ar => ar.Role).Select(ar => ar.Actor.Name)));
        CreateMap<FilmCreateRequest, FilmDTO>().ReverseMap();
    }
}
