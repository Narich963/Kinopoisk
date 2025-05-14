using AutoMapper;
using Kinopoisk.Core.Enitites;
using Kinopoisk.MVC.Models;
using Kinopoisk.Services.DTO;

namespace Kinopoisk.MVC.Initializers;

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
        CreateMap<FilmsViewModel, FilmDTO>()
            .ReverseMap();
            //.ForMember(dest => dest.DirectorName, opt => opt.MapFrom(src => src.Director.Name))
            //.ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Genres.Select(g => g.Name)))
            //.ForMember(dest => dest.ActorRoles, opt => opt.MapFrom(src => src.ActorRoles.OrderBy(ar => ar.Role).Select(ar => ar.Actor.Name)));
    }
}
