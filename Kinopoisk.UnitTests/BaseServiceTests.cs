using AutoMapper;
using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Filters;
using Kinopoisk.Core.Interfaces.Repositories;
using Kinopoisk.MVC.Models;
using Kinopoisk.Services.Services;
using Kinopoisk.UnitTests.Mapper;
using Microsoft.Extensions.Logging;
using Moq;

namespace Kinopoisk.UnitTests;

public class BaseServiceTests
{
    #region GetById
    [Fact]
    public async Task Genres_GetById_Successful()
    {
        // Arrange
        var mocks = Arrange<Genre, DataTablesRequestModel, GenreService>();
        var genre = new Genre
        {
            Id = 1,
            Name = "Test"
        };
        var repo = new Mock<IRepository<Genre, DataTablesRequestModel>>();
        mocks.uow.Setup(m => m.GetGenericRepository<Genre, DataTablesRequestModel>()).Returns(repo.Object);
        repo.Setup(r => r.GetByIdAsync(1, null)).ReturnsAsync(genre);

        var service = new GenreService(mocks.uow.Object, mocks.mapper, mocks.logger.Object);

        // Act
        var result = await service.GetByIdAsync(1);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("Test", result.Value.Name);
    }
    [Fact]
    public async Task Films_GetById_Successful()
    {
        // Arrange
        var mocks = Arrange<Film, FilmFilter, FilmService>();
        var film = new Film
        {
            Id = 1,
            Name = "Test"
        };
        var repo = new Mock<IFilmRepository>();
        mocks.uow.Setup(m => m.GetSpecificRepository<Film>()).Returns(repo.Object);
        repo.Setup(f => f.GetByIdAsync(1)).ReturnsAsync(film);

        var service = new FilmService(mocks.uow.Object, mocks.mapper, mocks.logger.Object);

        // Act
        var result = await service.GetByIdAsync(1);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("Test", result.Value.Name);
    }
    #endregion

    #region GetPagedAsync
    [Fact]
    public async Task Genres_GetPagedAsync_Successful()
    {
        // Arrange
        var mocks = Arrange<Genre, DataTablesRequestModel, GenreService>();
        List<Genre> genres = new();
        for (int i = 0; i < 50; i++)
        {
            genres.Add(new Genre { Id = i, Name = $"Test{i}" });
        }
        var filter = GetFilter();

        DataTablesResult<Genre> requiredResult = new()
        {
            Data = genres.Where(g => g.Id.ToString().Contains("7") || g.Name.Contains("7")).OrderByDescending(g => g.Id).Take(5).ToList(),
            RecordsTotal = 20,
            RecordsFiltered = 5
        };
        var repo = new Mock<IRepository<Genre, DataTablesRequestModel>>();
        mocks.uow.Setup(m => m.GetGenericRepository<Genre, DataTablesRequestModel>()).Returns(repo.Object);
        repo.Setup(r => r.GetPagedAsync(filter, null)).ReturnsAsync(requiredResult);

        var service = new GenreService(mocks.uow.Object, mocks.mapper, mocks.logger.Object);

        // Act
        var result = await service.GetPagedAsync(filter);

        // Assert
        Assert.Equal(5, result.Data.Count());
        Assert.Equal("Test47", result.Data.First().Name);
    }
    [Fact]
    public async Task Films_GetPagedAsync_Successful()
    {
        // Arrange
        var mocks = Arrange<Film, FilmFilter, FilmService>();
        List<Film> films = new();
        for (int i = 0; i < 50; i++)
        {
            films.Add(new Film { Id = i, Name = $"Test{i}" });
        }
        var filter = GetFilter();
        var filmFilter = new FilmFilter()
        {
            Start = filter.Start,
            Draw = filter.Draw,
            Length = filter.Length,
            Columns = filter.Columns,
            Order = filter.Order,
            Search = filter.Search
        };

        DataTablesResult<Film> requiredResult = new()
        {
            Data = films
                .Where(g => g.Id.ToString().Contains("7") || g.Name.Contains("7"))
                .OrderByDescending(g => g.Id)
                .Take(5)
                .ToList(),
            RecordsTotal = 50,
            RecordsFiltered = 5
        };
        var repo = new Mock<IFilmRepository>();
        mocks.uow.Setup(m => m.GetGenericRepository<Film, FilmFilter>()).Returns(repo.Object);
        repo.Setup(r => r.GetPagedAsync(filmFilter, null)).ReturnsAsync(requiredResult);

        var service = new FilmService(mocks.uow.Object, mocks.mapper, mocks.logger.Object);

        // Act
        var result = await service.GetPagedAsync(filmFilter);

        // Assert
        Assert.Equal(5, result.Data.Count());
        Assert.Equal("Test47", result.Data.First().Name);
    }
    #endregion

    #region AddAsync
    [Fact]
    public async Task Genres_AddAsync_Successful()
    {
        // Arrange
        var mocks = Arrange<Genre, DataTablesRequestModel, GenreService>();
        var genre = new Genre
        {
            Id = 1,
            Name = "Test"
        };
        var repo = new Mock<IRepository<Genre, DataTablesRequestModel>>();
        mocks.uow.Setup(m => m.GetGenericRepository<Genre, DataTablesRequestModel>()).Returns(repo.Object);
        repo.Setup(r => r.AddAsync(It.IsAny<Genre>())).ReturnsAsync((Genre g) => g);
        mocks.uow.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var service = new GenreService(mocks.uow.Object, mocks.mapper, mocks.logger.Object);
        var genreDTO = mocks.mapper.Map<GenreDTO>(genre);

        // Act
        var result = await service.AddAsync(genreDTO);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Test", result.Value.Name);
        repo.Verify(r => r.AddAsync(It.IsAny<Genre>()), Times.Once());
        mocks.uow.Verify(u => u.SaveChangesAsync(), Times.Once());
    }
    #endregion

    private (IMapper mapper, Mock<IUnitOfWork> uow, Mock<ILogger<TService>> logger) 
        Arrange<T, TRequest, TService>() 
        where TRequest : DataTablesRequestModel 
        where T : class
    {
        var mapperConfig = new MapperConfiguration(opts =>
        {
            opts.AddProfile<MapperProfile>();
        });
        var mapper = mapperConfig.CreateMapper();

        var uow = new Mock<IUnitOfWork>();
        var logger = new Mock<ILogger<TService>>();

        return (mapper, uow, logger);
    }
    private DataTablesRequestModel GetFilter()
    {
        DataTablesRequestModel filter = new()
        {
            Draw = 1,
            Start = 0,
            Length = 5,
            Columns = new()
            {
                new DataTablesRequestModel.ColumnModel{
                    Data = "id",
                    Name = "",
                    Orderable = true
                }
            },
            Order = new()
            {
                new DataTablesRequestModel.OrderModel
                {
                    Column = 0,
                    Dir = "desc"
                }
            },
            Search = new()
            {
                Value = "7",
                Regex = false
            }
        };
        return filter;
    }
}
