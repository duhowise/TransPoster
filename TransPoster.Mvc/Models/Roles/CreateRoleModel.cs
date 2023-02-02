using System.ComponentModel.DataAnnotations;

namespace TransPorter.Mvc.Models.Roles;

public class CreateRoleModel
{
    [Required(ErrorMessage = "Role is required")]
    public string Name { get; set; }
}