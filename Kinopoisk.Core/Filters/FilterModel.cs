using System.Linq.Expressions;

namespace Kinopoisk.Core.Filters;

public class FilterModel<T>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public string? SortField { get; set; }
    public bool IsAscending { get; set; } = true;

    public List<Expression<Func<T, bool>>>? Predicates { get; set; }
}
