namespace Kinopoisk.Core.Enitites.Localization;

public class Localization
{
    public int Id { get; set; }
    public string CultureInfo { get; set; }
    public string Value { get; set; }

    public int LocalizationSetId { get; set; }
    public LocalizationSet? LocalizationSet { get; set; }
}
