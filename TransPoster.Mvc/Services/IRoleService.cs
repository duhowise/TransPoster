using Microsoft.AspNetCore.Identity;
using TransPoster.Data.Models;
using TransPoster.Mvc.Models.Roles;

namespace TransPoster.Mvc.Services;

public interface IRoleService
{
    Task<IEnumerable<ApplicationRole>> FindAllAsync();
    Task<ApplicationRole?> CreateAsync(CreateRoleModel createRoleModel);
    Task<ApplicationRole> UpdateRoleAsync(string id, ApplicationRole identityRole);
    Task<ApplicationRole?> GetIdentityRoleAsync(string id);
    Task DeleteRoleAsync(string id);
    Task<ApplicationRole?> FindByName(string name);
}