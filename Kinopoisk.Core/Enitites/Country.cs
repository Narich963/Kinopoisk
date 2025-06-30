using Kinopoisk.Core.Enitites.Localization;
using System.ComponentModel.DataAnnotations;

namespace Kinopoisk.Core.Enitites;

public class Country
{
    public int Id { get; set; }

    [MaxLength(10)]
    public string IsoCode { get; set; }

    public int? NameId { get; set; }
    public virtual LocalizationSet? Name { get; set; }

    public virtual List<Film> Films { get; set; }

    public Country()
    {
        Films = new();
    }
}
