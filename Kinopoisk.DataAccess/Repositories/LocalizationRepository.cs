using CSharpFunctionalExtensions;
using Kinopoisk.Core.Enitites.Localization;
using Kinopoisk.Core.Enums;
using Kinopoisk.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kinopoisk.DataAccess.Repositories;

public class LocalizationRepository : ILocalizationRepository
{
    private readonly KinopoiskContext _context;

    public LocalizationRepository(KinopoiskContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Localization>> GetLocalizations(int localizationSetId, PropertyEnum property = PropertyEnum.Name)
    {
        var localizations = await _context.Localizations
                .Where(l => l.LocalizationSetId == localizationSetId && l.Property == property)
                .ToListAsync();
        return localizations;
    }
    public async Task<Result> UpdateLocalizations(List<Localization> localizations, int localizationSetId)
    {
        localizations.ForEach(l => l.LocalizationSetId = localizationSetId);
        var property = localizations.First().Property;

        var existingLocalizations = await _context.Localizations
            .Where(l => l.LocalizationSetId == localizationSetId && l.Property == property)
            .ToListAsync();

        if (localizations.Count > existingLocalizations.Count)
        {
            var localizationsToAdd = localizations.Except(existingLocalizations);
            await _context.AddAsync(localizationsToAdd);
        }
        foreach (var loc in existingLocalizations)
        {
            loc.Value = localizations.FirstOrDefault(l => l.Culture == loc.Culture && l.Property == loc.Property)?.Value;
        }

        _context.UpdateRange(existingLocalizations);

        return Result.Success();
    }
}
