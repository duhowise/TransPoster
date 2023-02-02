using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TransPoster.Data.Models;
using TransPoster.Mvc.Models.Users;

namespace TransPoster.Mvc.Services;

public class UserService : IUserService
{
    private readonly IUserStore<ApplicationUser> _userStore;
    private readonly IUserEmailStore<ApplicationUser> _emailStore;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IRoleService _roleService;

    public UserService(
        IUserStore<ApplicationUser> userStore,
        IUserEmailStore<ApplicationUser> emailStore,
        UserManager<ApplicationUser> userManager,
        IRoleService roleService)
    {
        _userStore = userStore;
        _emailStore = emailStore;
        _userManager = userManager;
        _roleService = roleService;
    }

    public async Task<IEnumerable<ApplicationUser>> FindAllUsersAsync() => await _userManager.Users.Include(u => u.Roles).ToListAsync();

    public async Task<ApplicationUser> CreateUserAsync(CreateUserModel body)
    {
        var role = await _roleService.FindByName(body.Role);
        if (role is null) throw new Exception("Role does not exist");

        var user = new ApplicationUser();

        await _userStore.SetUserNameAsync(user, body.UserName, CancellationToken.None);
        await _emailStore.SetEmailAsync(user, body.Email, CancellationToken.None);

        var result = await _userManager.CreateAsync(user, body.Password);

        if (result.Succeeded) return user;

        var error = result.Errors.ToList().First();
        throw new Exception(error.Description);
    }

    public async Task<ApplicationUser> AddRoleToUser(CreateUserModel body)
    {
        var role = await _roleService.FindByName(body.Role);
        if (role is null) throw new Exception("Role does not exist");

        var user = new ApplicationUser();

        await _userStore.SetUserNameAsync(user, body.UserName, CancellationToken.None);
        await _emailStore.SetEmailAsync(user, body.Email, CancellationToken.None);

        var result = await _userManager.CreateAsync(user, body.Password);

        if (result.Succeeded) return user;

        var error = result.Errors.ToList().First();
        throw new Exception(error.Description);
    }
}