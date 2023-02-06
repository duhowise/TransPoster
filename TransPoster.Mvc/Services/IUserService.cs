using TransPoster.Data.Identity;
using TransPoster.Mvc.Models.Users;

namespace TransPoster.Mvc.Services;

public interface IUserService
{
    public Task<IEnumerable<ApplicationUser>> FindAllUsersAsync();

    public Task<ApplicationUser> CreateUserAsync(CreateUserModel body);

    public Task AddRoleToUser(AddRoleToUserModel body);
}