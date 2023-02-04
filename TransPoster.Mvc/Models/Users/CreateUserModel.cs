using System.ComponentModel.DataAnnotations;

namespace TransPoster.Mvc.Models.Users;

public class CreateUserModel
{
    [Required]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }


    [Required]
    public string Password { get; set; }

    [Required]
    public string Role { get; set; }
}