using Kinopoisk.Core.Enitites;
using Kinopoisk.DataAccess.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Kinopoisk.DataAccess.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly KinopoiskContext _context;
    private IRepository<Film> _filmRepository;
    private UserManager<User> _userManager;

    public UnitOfWork(KinopoiskContext context, IRepository<Film> filmRepository, UserManager<User> userManager)
    {
        _context = context;
        _filmRepository = filmRepository;
        _userManager = userManager;
    }

    public IRepository<Film> FilmRepository => _filmRepository;

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
