using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using TransPoster.Shared.Enums;
using TransPoster.Shared.Interfaces;

namespace TransPoster.Data.Models;

public class ApplicationUser : IdentityUser<Guid>, IAuditableEntity
{
    [MaxLength(100), Required]
    public string FirstName { get; set; } = null!;
    [MaxLength(100), Required]
    public string LastName { get; set; } = null!;
    [Required]
    public Gender Gender { get; set; }
    public Guid CreatedUserId { get; set; }
    public DateTime CreatedOn { get; set; }
    public Guid? ModifiedUserId { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public bool IsActive { get; set; }
    public ApplicationUser() { }

    public ApplicationUser(string firstName, string lastName, Gender gender, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Gender = gender;
        Email = email;
    }
}
