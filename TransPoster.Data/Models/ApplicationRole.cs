using Microsoft.AspNetCore.Identity;

namespace TransPoster.Data.Models;

public sealed class ApplicationRole :IdentityRole<string>
{
    public ApplicationRole()
    {
        
    }
    public ApplicationRole(string roleName) : base(roleName)
    {
        Name = roleName;
    }
}