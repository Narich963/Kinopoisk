using Kinopoisk.Core.Enitites.Localization;
using Kinopoisk.Core.Enums;

namespace Kinopoisk.DataAccess.Extensions;

public static class LocalizationExtension
{
    public static string? GetLocalizationValue<T>(this T localizationSet, PropertyEnum property, string culture = "en")
        where T : LocalizationSet
    {
        CultureEnum cultureEnum = culture switch
        {
            "en" => CultureEnum.En,
            "ru" => CultureEnum.Ru,
            _ => CultureEnum.En
        };
        var value = localizationSet.Localizations
            .FirstOrDefault(l => l.Culture == cultureEnum && l.Property == property)?.Value 
            ?? null;

        return value;
    }
}
