using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Kinopoisk.MVC.Models;

public class CountryViewModel
{
    public int Id { get; set; }

    [ValidateNever]
    public string Name { get; set; }
    public string IsoCode { get; set; }
    public string? Flag { get; set; }

    public List<LocalizationViewModel> NameLocalizations { get; set; }
    public bool? IsNew { get; set; }
}
