using Microsoft.AspNetCore.Identity;
using TransPoster.Mvc.Models.Roles;

namespace TransPoster.Mvc.Services;

public interface IRoleService
{
    public Task<IEnumerable<IdentityRole>> FindAllAsync();
    public Task<IdentityRole?> CreateAsync(CreateRoleModel createRoleModel);

    public Task<IdentityRole?> FindByName(string name);
}