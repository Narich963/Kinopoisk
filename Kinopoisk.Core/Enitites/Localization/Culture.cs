using System.ComponentModel.DataAnnotations;

namespace Kinopoisk.Core.Enitites.Localization;

public class Culture
{
    [Key]
    public string Code { get; set; }
    public string Name { get; set; }
}
