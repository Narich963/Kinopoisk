using AutoMapper;
using Kinopoisk.MVC.Models;
using Kinopoisk.Services.DTO;
using Kinopoisk.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Kinopoisk.MVC.Controllers;

public class CommentController : Controller
{
    private readonly ILogger<CommentController> _logger;
    private readonly ICommentService _commentsService;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public CommentController(ILogger<CommentController> logger, ICommentService commentsService, IMapper mapper, IUserService userService)
    {
        _logger = logger;
        _commentsService = commentsService;
        _mapper = mapper;
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetComments(int? filmId)
    {
        var commentsResult = await _commentsService.GetAllByFilmAsync(filmId);
        if (commentsResult.IsFailure)
        {
            _logger.LogError(commentsResult.Error);
            return NotFound();
        }
        var commentsVm = _mapper.Map<List<CommentViewModel>>(commentsResult.Value);
        return Json(commentsVm);
    }

    [HttpPost]
    public async Task<IActionResult> AddComment(CommentViewModel commentVm)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var commentDto = _mapper.Map<CommentDTO>(commentVm);
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
        return Ok(new {success = true});
    }
}
