using Microsoft.AspNetCore.Identity;
using TransPoster.Data.Models;

namespace TransPoster.Data.Identity;

public class ApplicationUser : IdentityUser
{
    public DateTime? LastLogin { get; set; }

    public bool IsActive { get; set; }

    public DateTime? PasswordUpdatedAt { get; set; }

    public ICollection<UserHistory> UserHistory { get; set; }

}
