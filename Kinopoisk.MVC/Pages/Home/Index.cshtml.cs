using AutoMapper;
using Kinopoisk.Core.Filters;
using Kinopoisk.MVC.Models;
using Kinopoisk.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using static Kinopoisk.MVC.Models.DataTablesRequestModel;

namespace Kinopoisk.MVC.Pages.Home;

public class IndexModel : PageModel
{
    private readonly IFilmService _filmService;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _cache;
    private readonly MemoryCacheEntryOptions _cacheEntryOptions = new MemoryCacheEntryOptions
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
    };

    public IndexModel(IFilmService filmService, IMapper mapper, IMemoryCache cache)
    {
        _filmService = filmService;
        _mapper = mapper;
        _cache = cache;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnGetGetTopFilmsByDate()
    {
        const string cacheKey = "TopFilmsByDate";
        var cachedFilms = await GetCachedFilms(cacheKey, "publishDate", true);
        return new JsonResult(new { dateFilms = cachedFilms });
    }

    public async Task<IActionResult> OnGetGetTopFilmsByRate()
    {
        const string cacheKey = "TopFilmsByRate";
        var cachedFilms = await GetCachedFilms(cacheKey, "sitesRating", false);
        return new JsonResult(new { rateFilms = cachedFilms });
    }

    [NonAction]
    public async Task<List<FilmsViewModel>> GetCachedFilms(string cacheKey, string columnName, bool isAscenging)
    {
        if (!_cache.TryGetValue(cacheKey, out List<FilmsViewModel> cachedFilms))
        {
            var filter = new FilmFilter()
            {
                Draw = 1,
                Start = 0,
                Length = 10,
                Order = new List<OrderModel>
            {
                new OrderModel { Column = 0, Dir = isAscenging ? "asc" : "desc" }
            },
                Columns = new()
            {
                new ColumnModel{Data = columnName, Name = "", Orderable = true}
            }
            };

            var result = await _filmService.GetPagedAsync(filter);
            cachedFilms = _mapper.Map<List<FilmsViewModel>>(result.Data);

            _cache.Set(cacheKey, cachedFilms, _cacheEntryOptions);
        }
        
        return cachedFilms;
    }
}
