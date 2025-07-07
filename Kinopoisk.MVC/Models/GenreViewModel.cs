using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Kinopoisk.MVC.Models;

public class GenreViewModel
{
    public int Id { get; set; }

    [ValidateNever]
    public string Name { get; set; }
    public List<LocalizationViewModel> NameLocalizations { get; set; }

    public bool? IsNew { get; set; }
}
