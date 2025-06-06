using AutoMapper;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Enitites;

namespace Kinopoisk.UnitTests.Mapper;

public class MapperProfile : Profile
{

    public MapperProfile()
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
    }

    private void CreateCountriesMap()
    {
        CreateMap<CountryDTO, Country>().ReverseMap();
    }

    private void CreateGenresMap()
    {
        CreateMap<GenreDTO, Genre>().ReverseMap();

        CreateMap<FilmGenreDTO, FilmGenre>().ReverseMap();
    }

    private void CreateFilmEmployeesMap()
    {
        CreateMap<FilmEmployeeDTO, FilmEmployee>().ReverseMap();

        CreateMap<FilmEmployeeRoleDTO, FilmEmployeeRole>().ReverseMap();
    }
    private void CreateFilmsMap()
    {
        CreateMap<FilmDTO, Film>().ReverseMap();
    }
    private void CreateCommentsMap()
    {
        CreateMap<CommentDTO, Comment>().ReverseMap();
    }
}
