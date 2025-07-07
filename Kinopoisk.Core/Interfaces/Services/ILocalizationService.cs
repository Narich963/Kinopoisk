using Kinopoisk.Core.DTO.Localization;
using Kinopoisk.Core.Enums;

namespace Kinopoisk.Core.Interfaces.Services;

public interface ILocalizationService
{
    Task<IEnumerable<LocalizationDTO>> GetEmptyLocalizations(PropertyEnum property);
    Task<IEnumerable<LocalizationDTO>> GetLocalizations(int localizationSetId, PropertyEnum property = PropertyEnum.Name);
    Task UpdateLocalizations(List<LocalizationDTO> localizationsDTO, int localizationSetId);
}
