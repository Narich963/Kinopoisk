using AutoMapper;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Enitites;
using Kinopoisk.MVC.Models;
using Kinopoisk.Services.Interfaces;
using Kinopoisk.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kinopoisk.MVC.Pages.Films;

public class DetailsModel : PageModel
{
    private readonly IFilmService _filmsService;
    private readonly IMapper _mapper;
    private readonly ILogger<DetailsModel> _logger;
    private readonly ICommentService _commentsService;
    private readonly IUserService _userService;

    public DetailsModel(IFilmService filmService, IMapper mapper, ILogger<DetailsModel> logger, ICommentService commentsService, IUserService userService)
    {
        _filmsService = filmService;
        _mapper = mapper;
        _logger = logger;
        _commentsService = commentsService;
        _userService = userService;
    }

    public FilmsViewModel Film { get; set; } = new();

    [BindProperty]
    public CommentViewModel Comment { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var filmDto = await _filmsService.GetByIdAsync(id);
        if (filmDto.IsFailure)
        {
            _logger.LogError(filmDto.Error);
            return NotFound();
        }
        Film = _mapper.Map<FilmsViewModel>(filmDto.Value);

        return Page();
    }

    public async Task<IActionResult> OnGetGetCommentsAsync(int? filmId)
    {
        var commentsResult = await _commentsService.GetAllByFilmAsync(filmId);
        if (commentsResult.IsFailure)
        {
            _logger.LogError(commentsResult.Error);
            return NotFound();
        }
        var commentsVm = _mapper.Map<List<CommentViewModel>>(commentsResult.Value);
        return new JsonResult(commentsVm);
    }

    public async Task<IActionResult> OnPostAddCommentAsync()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var commentDto = _mapper.Map<CommentDTO>(Comment);
        var userDto = await _userService.GetUserAsync(User);

        if (userDto.IsFailure)
        {
            _logger.LogError(userDto.Error);
            return BadRequest(userDto.Error);
        }

        commentDto.UserId = userDto.Value.Id;

        var result = await _commentsService.AddAsync(commentDto);
        if (result.IsFailure)
        {
            _logger.LogError(result.Error);
            return BadRequest(result.Error);
        }
        return new JsonResult(new { success = true });
    }
}
