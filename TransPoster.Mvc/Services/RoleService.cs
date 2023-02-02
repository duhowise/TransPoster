using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TransPoster.Mvc.Models.Roles;

namespace TransPoster.Mvc.Services;

public class RoleService : IRoleService
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleService(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<IEnumerable<IdentityRole>> FindAllAsync() => await _roleManager.Roles.ToListAsync();

    public async Task<IdentityRole?> CreateAsync(CreateRoleModel createRoleModel)
    {
        var roleExist = await _roleManager.RoleExistsAsync(createRoleModel.Name);

        if (roleExist)
        {
            throw new Exception($"{createRoleModel.Name} role already exists!");
        }


        var res = await _roleManager.CreateAsync(new IdentityRole(createRoleModel.Name));

        if (!res.Succeeded) throw new Exception("Failed to create role");

        return await _roleManager.FindByNameAsync(createRoleModel.Name);
    }
}