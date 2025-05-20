using CSharpFunctionalExtensions;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Filters;
using Kinopoisk.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Kinopoisk.DataAccess.Repositories;

public class CommentRepository : GenericRepository<Comment>, ICommentRepository
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

    public async Task<DataTablesResult<Comment>> GetAllByFilmAsync(CommentFilter filter)
    {
        var query = _context.Comments
            .Include(q => q.User)
            .Where(q => q.FilmId == filter.FilmId)
            .AsQueryable();

        Expression<Func<Comment, object>> orderBy = null;

        if (filter.Order != null && filter.Order.Count > 0)
        {
            var order = filter.Order[0];
            var columnName = filter.Columns[order.Column].Data;
            switch (columnName)
            {
                case "user.userName":
                    orderBy = c => c.User.UserName;
                    break;
                default:
                    orderBy = c => EF.Property<Comment>(c, ToPascaleCase(columnName));
                    break;
            }

            query = order.Dir == "asc"
                ? query.OrderBy(orderBy)
                : query.OrderByDescending(orderBy);
        }

        if (!string.IsNullOrEmpty(filter.Search?.Value))
        {
            string searchValue = filter.Search.Value.ToLower();
            query = query.Where(c => c.Text.ToLower().Contains(searchValue)
                || c.User.UserName.Contains(searchValue)
                || c.CreatedAt.ToString().ToLower().Contains(searchValue));
        }


        return await base.GetPagedAsync(filter, query);
    }

    public Task<Result<Comment>> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}
