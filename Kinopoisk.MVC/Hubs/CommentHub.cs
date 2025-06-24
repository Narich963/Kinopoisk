using Kinopoisk.Core.DTO;
using Kinopoisk.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Kinopoisk.MVC.Hubs;

public class CommentHub : Hub
{
    private readonly ICommentService _commentService;
    private readonly IUserService _userService;

    public CommentHub(ICommentService commentService, IUserService userService)
    {
        _commentService = commentService;
        _userService = userService;
    }

    public async Task Send(string message, int filmId)
    {
        var user = Context.User;
        var userResult = await _userService.GetUserAsync(user);
        if (userResult.IsFailure)
        {
            throw new HubException("User not authenticated");
        }
        
        var commentDto = new CommentDTO
        {
            FilmId = filmId,
            Text = message,
            UserId = userResult.Value.Id
        };

        var commentResult = await _commentService.AddAsync(commentDto);
        if (commentResult.IsFailure)
        {
            throw new HubException(commentResult.Error);
        }

        await Clients.All.SendAsync("Receive");
    }
}
