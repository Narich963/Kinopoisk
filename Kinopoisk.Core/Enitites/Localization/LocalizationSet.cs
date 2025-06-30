namespace Kinopoisk.Core.Enitites.Localization;

public class LocalizationSet
{
    public int Id { get; set; }
    public string Entity { get; set; }
    public string Property { get; set; }

    public virtual List<Localization> Localizations { get; set; } = new();
}
