using CSharpFunctionalExtensions;
using Kinopoisk.Core.DTO;
using System.Security.Claims;

namespace Kinopoisk.Services.Interfaces;

public interface IUserService
{
    Task<Result<UserDTO>> GetUserAsync(ClaimsPrincipal principal);
    Task<bool> IsExistingUser(string login);
}
