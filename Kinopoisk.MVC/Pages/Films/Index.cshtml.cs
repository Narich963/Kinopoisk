using AutoMapper;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Filters;
using Kinopoisk.MVC.Models;
using Kinopoisk.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq.Expressions;

namespace Kinopoisk.MVC.Pages.Films;

public class IndexModel : PageModel
{
    private readonly IFilmService _filmService;
    private readonly IMapper _mapper;

    public IndexModel(IFilmService filmService, IMapper mapper)
    {
        _filmService = filmService;
        _mapper = mapper;
    }

    [BindProperty(SupportsGet = true)]
    public FilmFilterModel FilterModel { get; set; } = new FilmFilterModel();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnGetGetFilmsAsync()
    {
        List<Expression<Func<FilmDTO, bool>>> predicates = new();
        
        if (FilterModel != null)
        {
            if (FilterModel.Name != null)
                predicates.Add(p => p.Name.Contains(FilterModel.Name));
            if (FilterModel.Year.HasValue)
                predicates.Add(p => p.PublishDate.Year == FilterModel.Year.Value);
            if (FilterModel.Country != null)
                predicates.Add(p => p.Country.Name.Contains(FilterModel.Country));
            if (FilterModel.ActorName != null)
                predicates.Add(p => p.Employees.Any(e => !e.IsDirector && e.FilmEmployee.Name.Contains(FilterModel.ActorName)));
            if (FilterModel.Director != null)
                predicates.Add(p => p.Employees.Any(e => e.IsDirector && e.FilmEmployee.Name.Contains(FilterModel.Director)));
        }


        FilterModel<FilmDTO> filterModel = new FilterModel<FilmDTO>
        {
            Page = FilterModel.Page,
            PageSize = FilterModel.PageSize,
            SortField = FilterModel.SortField,
            IsAscending = FilterModel.IsAscending,
            Predicates = predicates
        };

        var result = await _filmService.GetPagedAsync(filterModel);

        var filmsPaged = new PagedResult<FilmsViewModel>
        {
            PageSize = result.PageSize,
            TotalCount = result.TotalCount,
            TotalPages = result.TotalPages,
            CurrentPage = result.CurrentPage,
            Items = _mapper.Map<List<FilmsViewModel>>(result.Items)
        };

        return new JsonResult(filmsPaged.Items);
    }
}
