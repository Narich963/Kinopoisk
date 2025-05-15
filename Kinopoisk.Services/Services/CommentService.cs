using AutoMapper;
using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Enitites;
using Kinopoisk.Core.Interfaces.Repositories;
using Kinopoisk.Services.Interfaces;

namespace Kinopoisk.Services.Services;

public class CommentService : BaseService<Comment>, ICommentService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CommentService(IUnitOfWork uow, IMapper mapper) : base(uow.CommentsRepository)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CommentDTO>> GetAllAsync()
    {
        var comments = await _uow.CommentsRepository.GetAllAsync();
        var commentsDtos = _mapper.Map<IEnumerable<CommentDTO>>(comments);
        return commentsDtos;
    }

    public async Task<Result<IEnumerable<CommentDTO>>> GetAllByFilmAsync(int? filmId)
    {
        if (!filmId.HasValue)
            return Result.Failure<IEnumerable<CommentDTO>>("FilmId is null");

        var commentsResult = await _uow.CommentsRepository.GetAllByFilmAsync(filmId.Value);
        if (commentsResult.IsFailure)
            return Result.Failure<IEnumerable<CommentDTO>>(commentsResult.Error);

        var commentsDtos = _mapper.Map<IEnumerable<CommentDTO>>(commentsResult.Value);
        return Result.Success(commentsDtos);
    }

    public Task<Result<CommentDTO>> GetByIdAsync(int? id)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<CommentDTO>> AddAsync(CommentDTO dto)
    {
        var comment = _mapper.Map<Comment>(dto);
        var result = await _uow.CommentsRepository.AddAsync(comment);
        if (result.IsFailure)
            return Result.Failure<CommentDTO>(result.Error);

        return Result.Success(_mapper.Map<CommentDTO>(result.Value));
    }

    public async Task<Result<CommentDTO>> UpdateAsync(CommentDTO dto)
    {
        var comment = _mapper.Map<Comment>(dto);
        var result = await _uow.CommentsRepository.UpdateAsync(comment);

        if (result.IsFailure)
            return Result.Failure<CommentDTO>(result.Error);
        return Result.Success(_mapper.Map<CommentDTO>(result.Value));
    }

    public async Task<Result> DeleteAsync(int? id)
    {
        if (!id.HasValue)
            return Result.Failure("Id is null");

        var result = await _uow.CommentsRepository.DeleteAsync(id.Value);
        if (result.IsFailure)
            return Result.Failure(result.Error);
        return Result.Success();
    }
}
