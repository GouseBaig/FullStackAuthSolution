using System;

namespace FullStackAuth.API.Models;

public class UserInfo
{
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? Role { get; set; }
}
