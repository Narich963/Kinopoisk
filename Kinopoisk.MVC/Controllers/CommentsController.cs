using AutoMapper;
using Kinopoisk.MVC.Models;
using Kinopoisk.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Kinopoisk.MVC.Controllers;

public class CommentsController : Controller
{
    private readonly ILogger<CommentsController> _logger;
    private readonly ICommentsService _commentsService;
    private readonly IMapper _mapper;

    public CommentsController(ILogger<CommentsController> logger, ICommentsService commentsService, IMapper mapper)
    {
        _logger = logger;
        _commentsService = commentsService;
        _mapper = mapper;
    }

}
