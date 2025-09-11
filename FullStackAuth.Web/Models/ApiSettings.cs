namespace FullStackAuth.Web.Models;

public class ApiSettings
{
    public string BaseUrl { get; set; } = string.Empty;
    public Endpoints Endpoints { get; set; } = new();
}

public class Endpoints
{
    public string Login { get; set; } = "login";
    public string Profile { get; set; } = "profile";
}