using Microsoft.AspNetCore.Identity;

namespace TransPoster.Data.Models;

public class ApplicationUser : IdentityUser
{
    public DateTime PasswordUpdatedAt { get; set; }

    public ICollection<UserHistory> UserHistory { get; set; }
}