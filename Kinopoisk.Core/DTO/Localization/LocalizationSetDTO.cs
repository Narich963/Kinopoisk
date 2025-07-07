namespace Kinopoisk.Core.DTO.Localization;

public class LocalizationSetDTO
{
    public int Id { get; set; }
    public virtual List<LocalizationDTO> Localizations { get; set; } = new();
}
