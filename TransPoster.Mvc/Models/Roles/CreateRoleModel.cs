using System.ComponentModel.DataAnnotations;

namespace TransPorter.Mvc.Models.Roles;

public class CreateRoleModel
{
    [Required]
    public string Name { get; set; }
}