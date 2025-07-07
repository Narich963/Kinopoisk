using CSharpFunctionalExtensions;
using Kinopoisk.Core.Enitites.Localization;
using Kinopoisk.Core.Enums;

namespace Kinopoisk.Core.Interfaces.Repositories;

public interface ILocalizationRepository
{
    Task<Result> UpdateLocalizations(List<Localization> localizations, int localizationSetId);
    Task<IEnumerable<Localization>> GetLocalizations(int localizationSetId, PropertyEnum property = PropertyEnum.Name);
}
