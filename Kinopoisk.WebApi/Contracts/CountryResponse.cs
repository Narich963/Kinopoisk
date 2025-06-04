namespace Kinopoisk.WebApi.Contracts;

public class CountryResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string IsoCode { get; set; }
    public string? Flag { get; set; }
}
