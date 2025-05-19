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

    public UnitOfWork(KinopoiskContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public IFilmRepository FilmRepository => _filmRepository ??= new FilmRepository(_context);
    public ICommentRepository CommentsRepository => _commentsRepository ??= new CommentRepository(_context);
    public IRatingRepository RatingRepository => _ratingRepository ??= new RatingRepository(_context);
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
