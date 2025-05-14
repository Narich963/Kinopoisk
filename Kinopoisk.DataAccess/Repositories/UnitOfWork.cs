using Kinopoisk.Core.Enitites;
using Kinopoisk.DataAccess.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Kinopoisk.DataAccess.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly KinopoiskContext _context;
    private UserManager<User> _userManager;
    private IFilmRepository _filmRepository;
    private ICommentsRepository _commentsRepository;

    public UnitOfWork(KinopoiskContext context, IFilmRepository filmRepository, UserManager<User> userManager, ICommentsRepository commentsRepository)
    {
        _context = context;
        _filmRepository = filmRepository;
        _userManager = userManager;
        _commentsRepository = commentsRepository;
    }

    public IFilmRepository FilmRepository => _filmRepository;
    public ICommentsRepository CommentsRepository => _commentsRepository;
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
