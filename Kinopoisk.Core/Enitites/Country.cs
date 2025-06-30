using System.ComponentModel.DataAnnotations;

namespace Kinopoisk.Core.Enitites;

public class Country
{
    public int Id { get; set; }

    [MaxLength(50)]
    public string Name { get; set; }

    [MaxLength(10)]
    public string IsoCode { get; set; }

    public virtual List<Film> Films { get; set; }

    public Country()
    {
        Films = new();
    }
}
