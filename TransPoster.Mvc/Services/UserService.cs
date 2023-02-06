using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TransPoster.Data.Models;
using TransPoster.Mvc.Models.Users;

namespace TransPoster.Mvc.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IRoleService _roleService;

    public UserService(
        UserManager<ApplicationUser> userManager,
        IRoleService roleService)
    {
        _userManager = userManager;
        _roleService = roleService;
    }

    public async Task<IEnumerable<ApplicationUser>> FindAllUsersAsync() =>
        await _userManager.Users.Where(u => u.IsActive).Include(u => u.Roles).ToListAsync();

    public async Task<ApplicationUser> CreateUserAsync(CreateUserModel body)
    {
        var user = new ApplicationUser
        {
            UserName = body.UserName,
            Email = body.Email,
            IsActive = true,
            PasswordUpdatedAt = DateTime.Now
        };

        var result = await _userManager.CreateAsync(user, body.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, body.Role);
            await _userManager.UpdateSecurityStampAsync(user);
            return user;
        }

        var error = result.Errors.ToList().First();
        throw new Exception(error.Description);
    }

    public async Task AddRoleToUser(AddRoleToUserModel body)
    {
        var user = await _userManager.FindByIdAsync(body.UserId);
        await _userManager.AddToRoleAsync(user!, body.Role);
    }

    public async Task RemoveRoleFromUserAsync(RemoveRoleFromUserModel body)
    {
        var user = await _userManager.FindByIdAsync(body.UserId);

        await _userManager.RemoveFromRoleAsync(user, body.Role);
    }

    public async Task<ApplicationUser?> FindByIdAsync(string id)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(_ => _.Id == id);

        if (user is null) throw new Exception("User not found!");

        var roles = await _roleService.FindAllAsync();
        List<IdentityRole> userRoles = new();

        foreach (var role in roles)
        {
            var isRole = await _userManager.IsInRoleAsync(user, role.Name!);

            if (isRole) userRoles.Add(role);
        }

        user.Roles = userRoles;

        return user;
    }

    public async Task DeleteUserAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user is null) throw new Exception("User does not exist!");

        user.IsActive = false;

        await _userManager.UpdateAsync(user);
    }
}