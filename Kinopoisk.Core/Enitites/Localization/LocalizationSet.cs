using Kinopoisk.Core.Enums;

namespace Kinopoisk.Core.Enitites.Localization;

public class LocalizationSet
{
    public int Id { get; set; }
    public virtual List<Localization> Localizations { get; set; } = new();

    public void AddLocalization(PropertyEnum property, Dictionary<CultureEnum, string> values)
    {
        foreach (var (key, value) in values)
        {
            var localization = new Localization
            {
                Property = property,
                Culture = key,
                Value = value
            };
            Localizations.Add(localization);
        }
    }
}
