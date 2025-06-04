using AutoMapper;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Enitites;
using Kinopoisk.WebApi.Contracts;

namespace Kinopoisk.WebApi.Initializers;

public class MapperInitializer : Profile
{
    //public MapperInitializer()
    //{
    //    CreateFilmsMap();

    //    CreateMap<FilmEmployeeDTO, FilmEmployee>().ReverseMap();
    //    CreateMap<FilmEmployeeRoleDTO, FilmEmployeeRole>().ReverseMap();
    //    CreateMap<GenreDTO, Genre>().ReverseMap();
    //    CreateMap<UserDTO, User>().ReverseMap();
    //}
    //private void CreateFilmsMap()
    //{
    //    CreateMap<FilmDTO, Film>().ReverseMap();

    //    CreateMap<FilmDTO, Film>().ReverseMap();
    //    CreateMap<FilmResponse, FilmDTO>()
    //        .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => StringDurationToNumber(src.Duration)))
    //        .ForMember(dest => dest.Employees, opt => opt.MapFrom(src => src.Actors.Concat(new[] { src.Director })))
    //        .ReverseMap()
    //        .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => $"{Math.Floor(src.Duration / 60)}h {src.Duration % 60}min"))
    //        .ForMember(dest => dest.Actors, opt => opt.MapFrom(src => src.Employees.Where(ar => !ar.IsDirector).OrderBy(ar => ar.Role)))
    //        .ForMember(dest => dest.Director, opt => opt.MapFrom(src => src.Employees.FirstOrDefault(e => e.IsDirector)));
    //}

    public MapperInitializer()
    {
        CreateFilmsMap();
        CreateCommentsMap();

        CreateMap<FilmEmployeeDTO, FilmEmployee>().ReverseMap();
        CreateMap<FilmEmployeeRoleDTO, FilmEmployeeRole>().ReverseMap();
        CreateMap<FilmEmployeeResponse, FilmEmployeeDTO>().ReverseMap();

        CreateMap<GenreDTO, Genre>().ReverseMap();
        CreateMap<GenreResponse, GenreDTO>().ReverseMap();

        CreateMap<UserDTO, User>().ReverseMap();

        CreateMap<FilmEmployeeRoleResponse, FilmEmployeeRoleDTO>().ReverseMap();
        CreateMap<FilmEmployeeResponse, FilmEmployeeDTO>().ReverseMap();
        CreateMap<GenreResponse, GenreDTO>().ReverseMap();

        CreateMap<CountryDTO, Country>().ReverseMap();
        CreateMap<CountryResponse, CountryDTO>()
            .ReverseMap()
            .ForMember(dest => dest.Flag, opt => opt.Ignore());

        CreateMap<RatingDTO, Rating>().ReverseMap();
        CreateMap<RatingResponse, RatingDTO>().ReverseMap();

        CreateMap<FilmGenreDTO, FilmGenre>().ReverseMap();
        CreateMap<FilmGenreResponse, FilmGenreDTO>().ReverseMap();
    }
    private void CreateFilmsMap()
    {
        CreateMap<FilmDTO, Film>().ReverseMap();
        CreateMap<FilmResponse, FilmDTO>()
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => StringDurationToNumber(src.Duration)))
            .ForMember(dest => dest.Employees, opt => opt.MapFrom(src => src.Actors.Concat(new[] { src.Director })))
            .ReverseMap()
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => $"{Math.Floor(src.Duration / 60)}h {src.Duration % 60}min"))
            .ForMember(dest => dest.Actors, opt => opt.MapFrom(src => src.Employees.Where(ar => !ar.IsDirector).OrderBy(ar => ar.Role)))
            .ForMember(dest => dest.Director, opt => opt.MapFrom(src => src.Employees.FirstOrDefault(e => e.IsDirector)));
    }
    private void CreateCommentsMap()
    {
        CreateMap<CommentDTO, Comment>().ReverseMap();
        CreateMap<CommentResponse, CommentDTO>()
            .ReverseMap();
    }

    private int StringDurationToNumber(string durationStr)
    {
        var hoursIndex = durationStr.IndexOf("h");
        var minutesIndex = durationStr.IndexOf("m");
        int durationInt = 0;
        if (hoursIndex >= 0)
        {
            durationInt = Convert.ToInt32(durationStr.Substring(0, hoursIndex).Trim()) * 60;
        }
        if (minutesIndex >= 0)
        {
            durationInt += Convert.ToInt32(durationStr.Substring(hoursIndex + 1, minutesIndex - hoursIndex - 1).Trim());
        }
        return durationInt;
    }
}
