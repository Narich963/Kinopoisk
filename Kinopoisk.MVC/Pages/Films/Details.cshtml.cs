using AutoMapper;
using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Filters;
using Kinopoisk.Core.Interfaces.Services;
using Kinopoisk.MVC.Models;
using Kinopoisk.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kinopoisk.MVC.Pages.Films;

[IgnoreAntiforgeryToken]
public class DetailsModel : PageModel
{
    private readonly IFilmService _filmsService;
    private readonly IMapper _mapper;
    private readonly ILogger<DetailsModel> _logger;
    private readonly ICommentService _commentsService;
    private readonly IUserService _userService;
    private readonly IRatingService _ratingService;

    public DetailsModel(IFilmService filmService, IMapper mapper, 
        ILogger<DetailsModel> logger, ICommentService commentsService, 
        IUserService userService, IRatingService ratingService)
    {
        _filmsService = filmService;
        _mapper = mapper;
        _logger = logger;
        _commentsService = commentsService;
        _userService = userService;
        _ratingService = ratingService;
    }

    public FilmsViewModel Film { get; set; } = new();

    [BindProperty]
    public CommentViewModel Comment { get; set; } = new();

    [BindProperty]
    public RatingViewModel Rating { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var filmDto = await _filmsService.GetByIdAsync(id);
        if (filmDto.IsFailure)
        {
            _logger.LogError(filmDto.Error);
            return NotFound();
        }

        Film = _mapper.Map<FilmsViewModel>(filmDto.Value);

        if (filmDto.Value.SitesRating == 0)
        {
            var sitesRatingResult = await _ratingService.CalculateSitesRating(filmDto.Value.Id);

            if (sitesRatingResult.IsFailure)
                Film.SitesRating = 0;
            else
                Film.SitesRating = sitesRatingResult.Value;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostGetCommentsAsync([FromBody] CommentFilter filter)
    {
        if (filter == null)
            return BadRequest("Model is null");

        var commentsResult = await _commentsService.GetAllByFilmAsync(filter);
        if (commentsResult.IsFailure)
        {
            _logger.LogError(commentsResult.Error);
            return NotFound();
        }
        var commetsPaged = new DataTablesResult<CommentViewModel>
        {
            Draw = commentsResult.Value.Draw,
            RecordsTotal = commentsResult.Value.RecordsTotal,
            RecordsFiltered = commentsResult.Value.RecordsFiltered,
            Data = _mapper.Map<List<CommentViewModel>>(commentsResult.Value.Data)
        };
        return new JsonResult(commetsPaged);
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
    public async Task<IActionResult> OnGetGetUserRatingAsync(int? filmId)
    {
        if (!filmId.HasValue)
            return BadRequest("Film ID is required.");

        var userDto = await _userService.GetUserAsync(User);
        if (userDto.IsFailure)
        {
            if (User.Identity.IsAuthenticated)
            {
                _logger.LogError(userDto.Error);
                return BadRequest(userDto.Error);
            }
            return new JsonResult(0); 
        }

        var ratingResult = await _ratingService.GetUserRating(filmId.Value, userDto.Value.Id);
        if (ratingResult.IsFailure)
        {
            _logger.LogError(ratingResult.Error);
            return BadRequest(ratingResult.Error);
        }
        return new JsonResult(ratingResult.Value);
    }
    public async Task<IActionResult> OnPostAddOrEditRatingAsync()
    {
        var ratingDto = _mapper.Map<RatingDTO>(Rating);
        var userDto = await _userService.GetUserAsync(User);

        if (userDto.IsFailure)
        {
            _logger.LogError(userDto.Error);
            return BadRequest(userDto.Error);
        }

        ratingDto.UserId = userDto.Value.Id;

        var existResult = await _ratingService.GetUserRating(ratingDto.FilmId, ratingDto.UserId);

        if (existResult.IsFailure)
            return BadRequest(existResult.Error);

        var result = existResult.Value < 0
            ? await _ratingService.AddAsync(ratingDto)
            : await _ratingService.UpdateAsync(ratingDto);

        var sitesRatingResult = await _ratingService.CalculateSitesRating(ratingDto.FilmId);

        if (result.IsFailure || sitesRatingResult.IsFailure)
        {
            _logger.LogError(result.Error);
            return BadRequest(result.Error);
        }
        return new JsonResult(new { success = true, newRating = sitesRatingResult.Value});
    }
}
