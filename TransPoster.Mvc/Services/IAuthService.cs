using TransPoster.Data.Models;

namespace TransPoster.Mvc.Services;

public interface IAuthService
{
    public Task<bool> UnlockUserAsync(string id);
    public Task<bool> LockUserAsync(string id);
    public Task<IEnumerable<ApplicationUser>> UsersWithLogin180DaysAgoAsync();
}