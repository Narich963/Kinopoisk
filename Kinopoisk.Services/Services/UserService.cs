using AutoMapper;
using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Interfaces.Repositories;
using Kinopoisk.Services.Interfaces;
using System.Security.Claims;

namespace Kinopoisk.Services.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public UserService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Result<UserDTO>> GetUserAsync(ClaimsPrincipal principal)
    {
        var user = await _uow.UserManager.GetUserAsync(principal);
        if (user == null)
            return Result.Failure<UserDTO>("User not found");

        var userDto = _mapper.Map<UserDTO>(user);
        return Result.Success(userDto);
    }

    public async Task<bool> IsExistingUser(string login)
    {
        var user = await _uow.UserManager.FindByNameAsync(login)
            ?? await _uow.UserManager.FindByEmailAsync(login);

        return user != null;
    }
}
