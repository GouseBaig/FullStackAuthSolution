namespace FullStackAuth.Web.Models;

public class UserProfile
{
    public int id { get; set; }
    public string username { get; set; } = string.Empty;
    public string role { get; set; } = string.Empty;
}
