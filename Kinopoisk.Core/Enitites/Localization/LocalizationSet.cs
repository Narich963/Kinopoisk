namespace Kinopoisk.Core.Enitites.Localization;

public class LocalizationSet
{
    public int Id { get; set; }
    public string Entity { get; set; }
    public string Property { get; set; }

    public List<Localization> Localizations { get; set; } = new();
}
