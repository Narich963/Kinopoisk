using AutoMapper;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Enitites;
using Kinopoisk.MVC.Models;

namespace Kinopoisk.MVC.Initializers;

public class MapperInitializer : Profile
{
    public MapperInitializer()
    {
        CreateFilmsMap();
        CreateCommentsMap();

        CreateMap<FilmEmployeeDTO, FilmEmployee>().ReverseMap();
        CreateMap<FilmEmployeeRoleDTO, FilmEmployeeRole>().ReverseMap();
        CreateMap<GenreDTO, Genre>().ReverseMap();
        CreateMap<UserDTO, User>().ReverseMap();

        CreateMap<FilmEmployeeRoleViewModel, FilmEmployeeRoleDTO>().ReverseMap();
        CreateMap<FilmEmployeeViewModel, FilmEmployeeDTO>().ReverseMap();
        CreateMap<GenreViewModel, GenreDTO>().ReverseMap();

        CreateMap<CountryDTO, Country>().ReverseMap();

        CreateMap<RatingDTO, Rating>().ReverseMap();
        CreateMap<RatingViewModel, RatingDTO>().ReverseMap();   
    }
    private void CreateFilmsMap()
    {
        CreateMap<FilmDTO, Film>().ReverseMap();
        CreateMap<FilmsViewModel, FilmDTO>()
            .ReverseMap()
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Genres.Select(g => g.Name)))
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => $"{Math.Floor(src.Duration / 60)}h {src.Duration % 60}min"))
            .ForMember(dest => dest.CountryFlagLink, opt => opt.MapFrom(src => CountryToFlagLink(src.Country.IsoCode)))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country.Name))
            .ForMember(dest => dest.Actors, opt => opt.MapFrom(src => src.Employees.Where(ar => !ar.IsDirector).OrderBy(ar => ar.Role).Select(ar => ar.FilmEmployee.Name)))
            .ForMember(dest => dest.DirectorName, opt => opt.MapFrom(src => src.Employees.FirstOrDefault(e => e.IsDirector).FilmEmployee.Name));
    }
    private void CreateCommentsMap()
    {
        CreateMap<CommentDTO, Comment>().ReverseMap();
        CreateMap<CommentViewModel, CommentDTO>()
            .ReverseMap();
    }
    private string CountryToFlagLink(string isoCode)
    {
        return $"https://flagcdn.com/24x18/{isoCode}.png";
    }
}
