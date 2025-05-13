using Kinopoisk.Core.Enitites;
using Microsoft.AspNetCore.Identity;

namespace Kinopoisk.DataAccess.Interfaces;

public interface IUnitOfWork : IDisposable
{
    public IRepository<Film> FilmRepository { get; }
    public UserManager<User> UserManager { get; }
    public Task SaveChangesAsync();
}
