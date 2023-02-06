using Microsoft.AspNetCore.Identity;

namespace TransPoster.Data.Identity
{
    public class UserIdentityUserRole : IdentityUserRole<string>
    {
        public ApplicationUser User { get; set; }
        public IdentityRole Role { get; set; }
    }
}