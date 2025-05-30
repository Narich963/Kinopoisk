using AutoMapper;
using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Filters;
using Kinopoisk.Core.Interfaces.Repositories;
using Kinopoisk.Services.Interfaces;

namespace Kinopoisk.Services.Services;

public class CommentService : BaseService<Comment, CommentDTO, CommentFilter>, ICommentService
{
    private readonly IMapper _mapper;
    private readonly ICommentRepository _repository;

    public CommentService(IUnitOfWork uow, IMapper mapper, ICommentRepository repository) : base(uow, mapper)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<IEnumerable<CommentDTO>> GetAllAsync()
    {
        var comments = await _repository.GetAllAsync();
        var commentsDtos = _mapper.Map<IEnumerable<CommentDTO>>(comments);
        return commentsDtos;
    }

    public async Task<Result<DataTablesResult<CommentDTO>>> GetAllByFilmAsync(CommentFilter filter)
    {
        if (!filter.FilmId.HasValue)
            return Result.Failure<DataTablesResult<CommentDTO>>("FilmId is null");

        var commentsResult = await _repository.GetAllByFilmAsync(filter);

        var commentDTOResult = new DataTablesResult<CommentDTO>
        {
            Draw = commentsResult.Draw,
            RecordsTotal = commentsResult.RecordsTotal,
            RecordsFiltered = commentsResult.RecordsFiltered,
            Data = _mapper.Map<List<CommentDTO>>(commentsResult.Data)
        };
        return Result.Success(commentDTOResult);
    }
}
