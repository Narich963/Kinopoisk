namespace Kinopoisk.MVC.Models;

public class CountryViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string IsoCode { get; set; }
    public string Flag { get; set; }

    public bool? IsNew { get; set; }
}
