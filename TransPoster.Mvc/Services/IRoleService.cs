using Microsoft.AspNetCore.Identity;
using TransPorter.Mvc.Models.Roles;

namespace TransPoster.Mvc.Services;

public interface IRoleService
{
    public Task<IEnumerable<IdentityRole>> FindAllAsync();
    public Task<IdentityRole?> CreateAsync(CreateRoleModel createRoleModel);
}