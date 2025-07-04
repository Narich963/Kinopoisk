using Kinopoisk.Core.Enitites.Localization;
using System.ComponentModel.DataAnnotations;

namespace Kinopoisk.Core.Enitites;

public class Country : LocalizationSet
{
    [MaxLength(10)]
    public string IsoCode { get; set; }


    public virtual List<Film> Films { get; set; }

    public Country()
    {
        Films = new();
    }
}
