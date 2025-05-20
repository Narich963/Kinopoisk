using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Enitites;
using Kinopoisk.MVC.Models;
using Kinopoisk.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kinopoisk.MVC.Pages.Genres;

public class IndexModel : PageModel
{
    private readonly IService<GenreDTO, DataTablesRequestModel> _genreService;
    public IndexModel(IService<GenreDTO, DataTablesRequestModel> genreService)
    {
        _genreService = genreService;
    }

    [BindProperty(SupportsGet = true)]
    public List<Genre> Genres { get; set; } = new List<Genre>();

    public async Task OnGetAsync([FromBody] DataTablesRequestModel model)
    {
        var genres = await _genreService.GetPagedAsync(model);
    }
}
