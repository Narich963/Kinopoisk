using Kinopoisk.Core.Enums;

namespace Kinopoisk.MVC.Models;

public class LocalizationViewModel
{
    public CultureEnum Culture { get; set; }
    public PropertyEnum Property { get; set; } = PropertyEnum.Name;
    public string Value { get; set; }
}
