using FullStackAuth.API.Models;

namespace FullStackAuth.API.Services;

public class InMemoryUserService : IUserService
{
    private readonly List<(int Id, string Username, string Password, string Role)> _users
            = new List<(int, string, string, string)>
        {
            (1, "test", "1234", "admin")
        };

    public UserInfo? ValidateUser(string username, string password)
    {
        var user = _users
                        .FirstOrDefault(x => x.Username == username && x.Password == password);
        if (user.Username == null) return null;
        return new UserInfo { Id = user.Id, Username = user.Username, Role = user.Role };
    }

    public UserInfo? GetByUsername(string username)
    {
        var user = _users
                        .FirstOrDefault(x => x.Username == username);
        if (user.Username == null) return null;
        return new UserInfo { Id = user.Id, Username = user.Username, Role = user.Role };
    }
}
