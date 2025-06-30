using AutoMapper;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Enitites;
using Kinopoisk.MVC.Models;
using System.Globalization;

namespace Kinopoisk.MVC.Initializers;

public class MapperInitializer : Profile
{
    public MapperInitializer()
    {
        CreateFilmsMap();
        CreateCommentsMap();
        CreateFilmEmployeesMap();
        CreateGenresMap();
        CreateCountriesMap();
        CreateRatingsMap();

        CreateMap<UserDTO, User>().ReverseMap();
    }

    private void CreateRatingsMap()
    {
        CreateMap<RatingDTO, Rating>().ReverseMap();
        CreateMap<RatingViewModel, RatingDTO>().ReverseMap();
    }

    private void CreateCountriesMap()
    {
        CreateMap<CountryDTO, Country>().ReverseMap();
        CreateMap<CountryViewModel, CountryDTO>()
            .ReverseMap()
            .ForMember(dest => dest.Flag, opt => opt.MapFrom(src => CountryToFlagLink(src.IsoCode)));
    }

    private void CreateGenresMap()
    {
        CreateMap<GenreDTO, Genre>().ReverseMap();
        CreateMap<GenreViewModel, GenreDTO>().ReverseMap();

        CreateMap<FilmGenreDTO, FilmGenre>().ReverseMap();
        CreateMap<FilmGenreViewModel, FilmGenreDTO>().ReverseMap();
    }

    private void CreateFilmEmployeesMap()
    {
        CreateMap<FilmEmployeeDTO, FilmEmployee>().ReverseMap();
        CreateMap<FilmEmployeeViewModel, FilmEmployeeDTO>().ReverseMap();

        CreateMap<FilmEmployeeRoleDTO, FilmEmployeeRole>().ReverseMap();
        CreateMap<FilmEmployeeRoleViewModel, FilmEmployeeRoleDTO>().ReverseMap();
    }
    private void CreateFilmsMap()
    {
        CreateMap<Film, FilmDTO>()
            .ForMember(dest => dest.Description,
                opt => opt.MapFrom(src => src.Description.Localizations.FirstOrDefault(x => x.CultureInfo == CultureInfo.CurrentUICulture.TwoLetterISOLanguageName).Value))
            .ForMember(dest => dest.Name,
                opt => opt.MapFrom(src => src.Name.Localizations.FirstOrDefault(x => x.CultureInfo == CultureInfo.CurrentUICulture.TwoLetterISOLanguageName).Value))
            .ReverseMap();
        CreateMap<FilmsViewModel, FilmDTO>()
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
        CreateMap<CommentViewModel, CommentDTO>()
            .ReverseMap();
    }
    private string CountryToFlagLink(string isoCode)
    {
        return $"https://flagcdn.com/24x18/{isoCode}.png";
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
