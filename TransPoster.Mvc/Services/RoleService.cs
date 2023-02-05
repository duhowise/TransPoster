using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TransPoster.Data.Models;
using TransPoster.Mvc.Models.Roles;

namespace TransPoster.Mvc.Services;

public class RoleService : IRoleService
{
    private readonly RoleManager<ApplicationRole> _roleManager;

    public RoleService(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<IEnumerable<ApplicationRole>> FindAllAsync() => await _roleManager.Roles.ToListAsync();
    public async Task<ApplicationRole?> CreateAsync(CreateRoleModel createRoleModel)
    {
        await EnsureRoleNameDoesNotExistAsync(createRoleModel.Name);
        var res = await _roleManager.CreateAsync(new ApplicationRole(createRoleModel.Name));

        if (!res.Succeeded) throw new Exception("Failed to create role");

        return await _roleManager.FindByNameAsync(createRoleModel.Name);
    }

    public async Task<ApplicationRole> UpdateRoleAsync(string id, ApplicationRole identityRole)
    {
        var existingRole = await _roleManager.FindByIdAsync(id);
        if (existingRole is null) throw new Exception("Role does not exist");
        await EnsureRoleNameDoesNotExistAsync($"{identityRole.Name}");
        var result = await _roleManager.UpdateAsync(identityRole);
        if (!result.Succeeded) throw new Exception("Failed to update role");
        return identityRole;
    }

    public async Task<ApplicationRole?> GetIdentityRoleAsync(string id) => await _roleManager.FindByIdAsync(id);
    public async Task DeleteRoleAsync(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if(role is null) throw new Exception("Role does not exist");
        var result = await _roleManager.DeleteAsync(role);
        if (!result.Succeeded) throw new Exception("Failed to delete role");
    }

    private async Task EnsureRoleNameDoesNotExistAsync(string roleName)
    {
        var roleExist = await _roleManager.RoleExistsAsync(roleName);

        if (roleExist) throw new Exception($"{roleName} role already exists!");
    }
    public async Task<ApplicationRole?> FindByName(string name) => await _roleManager.FindByNameAsync(name);
}