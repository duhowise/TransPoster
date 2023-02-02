using System.ComponentModel.DataAnnotations;

namespace TransPoster.Mvc.Models.Users;

public class AddRoleToUserModel
{

    [Required]
    public string UserId { get; set; }

    [Required]

    public string Role { get; set; }
}

