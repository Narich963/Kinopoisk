using AutoMapper;
using Kinopoisk.Core.DTO.Localization;
using Kinopoisk.Core.Enitites.Localization;
using Kinopoisk.Core.Enums;
using Kinopoisk.Core.Interfaces.Repositories;
using Kinopoisk.Core.Interfaces.Services;

namespace Kinopoisk.Services.Services;

public class LocalizationService : ILocalizationService
{
    private readonly ILocalizationRepository _repository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;

    public LocalizationService(IMapper mapper, IUnitOfWork uow)
    {
        _mapper = mapper;
        _uow = uow;
        _repository = _uow.GetSpecificRepository<Localization>() as ILocalizationRepository;
    }

    public async Task<IEnumerable<LocalizationDTO>> GetLocalizations(int localizationSetId, PropertyEnum property = PropertyEnum.Name)
    {
        var localizations =  await _repository.GetLocalizations(localizationSetId, property);

        var dtos = _mapper.Map<List<LocalizationDTO>>(localizations);
        return dtos;
    }
    public async Task<IEnumerable<LocalizationDTO>> GetEmptyLocalizations(PropertyEnum property)
    {
        var emptyLocalizations = new List<LocalizationDTO>();
        foreach (var culture in Enum.GetValues(typeof(CultureEnum)) as CultureEnum[])
        {
            emptyLocalizations.Add(new LocalizationDTO
            {
                Culture = culture,
                Property = property
            });
        }
        return emptyLocalizations;
    }
    public async Task UpdateLocalizations(List<LocalizationDTO> localizationsDTO, int localizationSetId)
    {
        var localizations = _mapper.Map<List<Localization>>(localizationsDTO);
        var result = await _repository.UpdateLocalizations(localizations, localizationSetId);
        if (result.IsSuccess)
            await _uow.SaveChangesAsync();
        else
            throw new Exception(result.Error);
    }
}
