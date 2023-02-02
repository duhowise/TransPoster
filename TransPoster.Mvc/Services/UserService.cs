using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TransPoster.Data;
using TransPoster.Data.Models;

namespace TransPoster.Mvc.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IEnumerable<ApplicationUser>> FindAllUsersAsync() => await _userManager.Users.Include(u => u.Roles).ToListAsync();

}