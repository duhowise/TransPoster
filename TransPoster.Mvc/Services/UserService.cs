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

    public async Task<IEnumerable<ApplicationUser>> FindAllUsersAsync() => await _userManager.Users.Include(u => u.Roles).ToListAsync();

    public async Task<ApplicationUser> CreateUserAsync(CreateUserModel body)
    {
        var role = await _roleService.FindByName(body.Role);
        if (role is null) throw new Exception("Role does not exist");

        var user = new ApplicationUser
        {
            UserName = body.UserName,
            Email = body.Email
        };

        var result = await _userManager.CreateAsync(user, body.Password);

        if (result.Succeeded) return user;

        var error = result.Errors.ToList().First();
        throw new Exception(error.Description);
    }

    public async Task AddRoleToUser(AddRoleToUserModel body)
    {
        var user = await _userManager.FindByIdAsync(body.UserId);

        await _userManager.AddToRoleAsync(user, body.Role);
    }

    public async Task RemoveRoleFromUserAsync(RemoveRoleFromUserModel body)
    {
        var user = await _userManager.FindByIdAsync(body.UserId);

        await _userManager.RemoveFromRoleAsync(user, body.Role);
    }
}