using Kinopoisk.Core.Enitites;
using Microsoft.AspNetCore.Identity;

namespace Kinopoisk.Core.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    public IFilmRepository FilmRepository { get; }
    public ICommentRepository CommentsRepository { get; }
    public UserManager<User> UserManager { get; }
    public Task SaveChangesAsync();
}
