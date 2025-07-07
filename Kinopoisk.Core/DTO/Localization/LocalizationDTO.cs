using Kinopoisk.Core.Enums;

namespace Kinopoisk.Core.DTO.Localization;

public class LocalizationDTO
{
    public CultureEnum Culture { get; set; }
    public PropertyEnum Property { get; set; }
    public string Value { get; set; }
}
