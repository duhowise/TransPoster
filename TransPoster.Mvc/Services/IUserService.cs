using TransPoster.Data.Models;
using TransPoster.Mvc.Models.Users;

namespace TransPoster.Mvc.Services;

public interface IUserService
{
    public Task<IEnumerable<ApplicationUser>> FindAllUsersAsync();

    public Task<ApplicationUser> CreateUserAsync(CreateUserModel body);
}