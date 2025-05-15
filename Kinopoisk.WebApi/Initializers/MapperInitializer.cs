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

        CreateMap<FilmEmployeeDTO, FilmEmployee>().ReverseMap();
        CreateMap<FilmEmployeeRoleDTO, FilmEmployeeRole>().ReverseMap();
        CreateMap<GenreDTO, Genre>().ReverseMap();
        CreateMap<UserDTO, User>().ReverseMap();
    }
    private void CreateFilmsMap()
    {
        CreateMap<FilmDTO, Film>().ReverseMap();
        CreateMap<FilmResponse, FilmDTO>()
            .ReverseMap()
            .ForMember(dest => dest.DirectorName, opt => opt.MapFrom(src => src.Employees.FirstOrDefault(e => e.IsDirector).FilmEmployee.Name))
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Genres.Select(g => g.Name)))
            .ForMember(dest => dest.ActorRoles, opt => opt.MapFrom(src => src.Employees.OrderBy(ar => ar.Role).Select(ar => ar.FilmEmployee.Name)));
        CreateMap<FilmCreateRequest, FilmDTO>().ReverseMap();
    }
}
