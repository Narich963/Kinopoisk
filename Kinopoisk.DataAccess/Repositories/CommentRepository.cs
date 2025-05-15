using CSharpFunctionalExtensions;
using Kinopoisk.Core.Enitites;
using Kinopoisk.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kinopoisk.DataAccess.Repositories;

public class CommentRepository : Repository<Comment>, ICommentRepository
{
    private readonly KinopoiskContext _context;
    public CommentRepository(KinopoiskContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Comment>> GetAllAsync()
    {
        var comments = await _context.Comments
            .Include(c => c.Film)
            .Include(c => c.User)
            .ToListAsync();
        return comments;
    }

    public async Task<Result<IEnumerable<Comment>>> GetAllByFilmAsync(int filmId)
    {
        var comments = await _context.Comments
            .Include(c => c.User)
            .Where(c => c.FilmId == filmId)
            .ToListAsync();

        return comments;
    }

    public Task<Result<Comment>> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}
