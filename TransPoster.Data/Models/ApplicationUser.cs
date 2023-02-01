using Microsoft.AspNetCore.Identity;

namespace TransPoster.Data.Models;

public class ApplicationUser : IdentityUser
{
    public DateTime? LastLogin { get; set; }

    public bool IsActive { get; set; }

    public DateTime PasswordUpdatedAt { get; set; }

    public ICollection<UserHistory> UserHistory { get; set; }
}