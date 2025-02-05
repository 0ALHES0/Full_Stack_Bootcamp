using System.ComponentModel.DataAnnotations;

namespace FilmLog.Models
{
    public class CreateUser
    {public string? UserName { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }

    }
}
