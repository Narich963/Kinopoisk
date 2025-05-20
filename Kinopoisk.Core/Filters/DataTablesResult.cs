namespace Kinopoisk.Core.Filters;

public class DataTablesResult<T>
{
    public IEnumerable<T> Data { get; set; } = new List<T>();
    public int RecordsTotal { get; set; }
    public int RecordsFiltered { get; set; }
    public int Draw { get; set; }
}
