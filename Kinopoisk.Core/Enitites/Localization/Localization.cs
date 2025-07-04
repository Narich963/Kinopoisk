using Kinopoisk.Core.Enums;

namespace Kinopoisk.Core.Enitites.Localization;

public class Localization
{
    public int Id { get; set; }
    public CultureEnum Culture { get; set; }
    public PropertyEnum Property { get; set; }
    public string Value { get; set; }

    public int LocalizationSetId { get; set; }
    public virtual LocalizationSet? LocalizationSet { get; set; }
}
