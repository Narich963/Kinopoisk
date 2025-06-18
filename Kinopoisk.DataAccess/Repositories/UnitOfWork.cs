using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Interfaces.Repositories;
using Kinopoisk.MVC.Models;
using Microsoft.AspNetCore.Identity;

namespace Kinopoisk.DataAccess.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly KinopoiskContext _context;
    private UserManager<User> _userManager;
    private Dictionary<Type, object> _repositories;

    public UnitOfWork(KinopoiskContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
        _repositories = new Dictionary<Type, object>()
        {
            { typeof(Film), new FilmRepository(_context) },
            { typeof(Comment), new CommentRepository(_context) },
            { typeof(Rating), new RatingRepository(_context)}
        };
    }
    public UserManager<User> UserManager => _userManager;
    public IRepository<TEntity, TRequest> GetGenericRepository<TEntity, TRequest>() 
        where TEntity : class
        where TRequest : DataTablesRequestModel
    {
        var type = typeof(TEntity);
        if (_repositories.TryGetValue(type, out var repo))
        {
            if (repo is IRepository<TEntity, TRequest> typedRepo)
            {
                return typedRepo;
            }
        }
        var repository = new GenericRepository<TEntity, TRequest>(_context);
        _repositories.Add(typeof(TEntity), repository);
        return repository;
    }
    public object GetSpecificRepository<TEntity>() where TEntity : class
    {
        var type = typeof(TEntity);
        if (_repositories.TryGetValue(type, out var repo))
        {
            return repo;
        }
        throw new Exception($"There is no specific repository for type {type}.");
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
