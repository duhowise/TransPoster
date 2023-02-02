using TransPoster.Data.Models;

namespace TransPoster.Mvc.Services;

public interface IUserService
{
    public Task<IEnumerable<ApplicationUser>> FindAllUsersAsync();

}