using CSharpFunctionalExtensions;
using Kinopoisk.Core.Enitites;
using Kinopoisk.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kinopoisk.DataAccess.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly KinopoiskContext _context;
    public CommentRepository(KinopoiskContext context)
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

    public async Task<Result<Comment>> AddAsync(Comment entity)
    {
        if (entity == null)
            return Result.Failure<Comment>("Comment is null");

        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();
        return Result.Success(entity);
    }

    public async Task<Result<Comment>> UpdateAsync(Comment entity)
    {
        if (entity == null)
            return Result.Failure<Comment>("Comment is null");

        _context.Update(entity);
        await _context.SaveChangesAsync();
        return Result.Success(entity);
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
            return Result.Failure("Comment not found");
        
        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
        return Result.Success("The comment is deleted.");
    }

    public Task<Result<Comment>> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}
