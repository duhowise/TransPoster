using System.ComponentModel.DataAnnotations;

namespace TransPoster.Mvc.Models.Users;

public class RemoveRoleFromUserModel
{
    [Required]
    public string UserId { get; set; }

    [Required]

    public string Role { get; set; }
}