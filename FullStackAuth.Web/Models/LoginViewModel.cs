using System;
using System.ComponentModel.DataAnnotations;

namespace FullStackAuth.Web.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public string? ErrorMessage { get; set; }
}
