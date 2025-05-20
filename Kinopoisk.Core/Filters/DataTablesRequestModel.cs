namespace Kinopoisk.MVC.Models;

public class DataTablesRequestModel
{
    public int Draw { get; set; }
    public int Start { get; set; }
    public int Length { get; set; }

    public SearchModel Search { get; set; }
    public List<OrderModel> Order { get; set; }
    public List<ColumnModel> Columns { get; set; }

    public class SearchModel
    {
        public string Value { get; set; }
        public bool Regex { get; set; }
    }

    public class OrderModel
    {
        public int Column { get; set; }
        public string Dir { get; set; }
    }

    public class ColumnModel
    {
        public string Data { get; set; }
        public string Name { get; set; }
        public bool Searchable { get; set; }
        public bool Orderable { get; set; }
        public SearchModel Search { get; set; }
    }
}

