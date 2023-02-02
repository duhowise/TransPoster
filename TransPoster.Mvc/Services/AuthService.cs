using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TransPoster.Data;
using TransPoster.Data.Models;

namespace TransPoster.Mvc.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<bool> UnlockUserAsync(string id) => await SetLockoutAsync(id, DateTime.Now - TimeSpan.FromMinutes(1));

    public async Task<bool> LockUserAsync(string id) => await SetLockoutAsync(id, new DateTime(2222, 06, 06));

    public async Task<IEnumerable<ApplicationUser>> UsersWithLogin180DaysAgoAsync() => await _context
            .Users
            .Where(u => u.LastLogin < DateTime.Now.AddDays(-180) && !u.LockoutEnabled)
            .ToListAsync();

    private async Task<bool> SetLockoutAsync(string id, DateTime endDate)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return false;       
        var enabled = await _userManager.SetLockoutEnabledAsync(user, false);

        var endDateSet = await _userManager.SetLockoutEndDateAsync(user, endDate);

        return enabled.Succeeded && endDateSet.Succeeded;
    }
}