namespace Kinopoisk.Core.DTO;

public class FilmsFilterDTO
{
    #region Pagination
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    #endregion

    #region Sorting
    public string SortField { get; set; } = "Id";
    public bool IsAscending { get; set; } = true;
    #endregion

    #region Filtering
    public string? Name { get; set; } = null;
    public int? Year { get; set; } = null;
    public string? Country { get; set; } = null;
    public string? ActorName { get; set; } = null;
    public string? Director { get; set; } = null;
    #endregion
}
