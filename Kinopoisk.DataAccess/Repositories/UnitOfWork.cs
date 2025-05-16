using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Kinopoisk.DataAccess.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly KinopoiskContext _context;
    private UserManager<User> _userManager;
    private IFilmRepository _filmRepository;
    private ICommentRepository _commentsRepository;
    private IRatingRepository _ratingRepository;

    public UnitOfWork(KinopoiskContext context, IFilmRepository filmRepository, 
        UserManager<User> userManager, ICommentRepository commentsRepository, 
        IRatingRepository ratingRepository)
    {
        _context = context;
        _filmRepository = filmRepository;
        _userManager = userManager;
        _commentsRepository = commentsRepository;
        _ratingRepository = ratingRepository;
    }

    public IFilmRepository FilmRepository => _filmRepository;
    public ICommentRepository CommentsRepository => _commentsRepository;
    public IRatingRepository RatingRepository => _ratingRepository;
    public UserManager<User> UserManager => _userManager;

    public void Dispose()
    {
        _context.Dispose();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
