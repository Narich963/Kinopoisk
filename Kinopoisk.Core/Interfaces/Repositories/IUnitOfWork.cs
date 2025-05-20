using Kinopoisk.Core.Enitites;
using Kinopoisk.MVC.Models;
using Microsoft.AspNetCore.Identity;

namespace Kinopoisk.Core.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    IRepository<TEntity, TRequest> GetRepository<TEntity, TRequest>() 
        where TEntity : class 
        where TRequest : DataTablesRequestModel;
    public UserManager<User> UserManager { get; }
    public Task SaveChangesAsync();
}
