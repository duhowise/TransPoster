using Microsoft.AspNetCore.Identity;
using TransPoster.Mvc.Models.Roles;

namespace TransPoster.Mvc.Services;

public interface IRoleService
{
    Task<IEnumerable<IdentityRole>> FindAllAsync();
    Task<IdentityRole?> CreateAsync(CreateRoleModel createRoleModel);
    Task<IdentityRole> UpdateRoleAsync(string id, IdentityRole identityRole);
    Task<IdentityRole?> GetIdentityRoleAsync(string id);
    Task DeleteRoleAsync(string id);
}