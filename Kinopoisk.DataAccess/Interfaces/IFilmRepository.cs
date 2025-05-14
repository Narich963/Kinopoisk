using Kinopoisk.Core.Enitites;

namespace Kinopoisk.DataAccess.Interfaces;

public interface IFilmRepository : IRepository<Film>
{
    IQueryable<Film> GetAllAsQueryable();
}
