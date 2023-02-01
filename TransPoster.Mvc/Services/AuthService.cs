using Microsoft.AspNetCore.Identity;
using TransPoster.Data.Models;

namespace TransPoster.Mvc.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> UnlockUserAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return false;       
        var enabled = await _userManager.SetLockoutEnabledAsync(user, false);

        var endDateSet = await _userManager.SetLockoutEndDateAsync(user, DateTime.Now - TimeSpan.FromMinutes(1));

        return enabled.Succeeded && endDateSet.Succeeded;
    }
}