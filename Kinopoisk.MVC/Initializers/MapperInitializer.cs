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

        CreateMap<ActorRoleViewModel, ActorRoleDTO>().ReverseMap();
        CreateMap<ActorViewModel, ActorDTO>().ReverseMap();
        CreateMap<DirectorViewModel, DirectorDTO>().ReverseMap();
        CreateMap<GenreViewModel, GenreDTO>().ReverseMap();
    }
    private void CreateFilmsMap()
    {
        CreateMap<FilmDTO, Film>().ReverseMap();
        CreateMap<FilmsViewModel, FilmDTO>()
            .ReverseMap()
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Genres.Select(g => g.Name)))
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => $"{Math.Floor(src.Duration / 60)}h {src.Duration % 60}min"))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => CountryToFlagLink(src.Country)))
            .ForMember(dest => dest.Actors, opt => opt.MapFrom(src => src.ActorRoles.OrderBy(ar => ar.Role).Select(ar => ar.Actor.Name)));
    }
    private string CountryToFlagLink(string country)
    {
        var countries = new Dictionary<string, string>
        {
            { "Russia", "RU" },
            { "USA", "US" },
            { "Great Britain", "GB" },
            { "France", "FR" },
            { "Germany", "DE" },
            { "Italy", "IT" },
            { "Spain", "ES" },
            { "Canada", "CA" },
            { "Australia", "AU" },
            { "Japan", "JP" },
            { "China", "Cn" },
            { "India", "IN" },
            { "South Korea", "KR" },
            { "Brazil", "BR" },
            { "Mexico", "MX" },
            { "Netherlands", "NL" },
            { "Sweden", "SE" },
            { "Norway", "NO" },
            { "Finland", "FI" },
            { "Denmark", "DK" },
            { "Belgium", "BE" },
            { "Switzerland", "CH" }
        };

        var countryCode = "";

        if (countries.ContainsKey(country))
            countryCode = countries[country].ToLower();

        return $"https://flagcdn.com/24x18/{countryCode}.png";
    }
}
