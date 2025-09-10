using FullStackAuth.API.Models;

namespace FullStackAuth.API.Services;

public interface IUserService
{
    UserInfo? ValidateUser(string username, string password);
    UserInfo? GetByUsername(string username);
}
