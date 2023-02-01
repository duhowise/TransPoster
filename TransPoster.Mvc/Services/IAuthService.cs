namespace TransPoster.Mvc.Services;

public interface IAuthService
{
    public Task<bool> UnlockUserAsync(string id);
}