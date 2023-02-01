using Microsoft.AspNetCore.Identity;
using TransPoster.Data.Models;

namespace TransPoster.Mvc.Validators.Auth;

public class ReplacePasswordValidator : IPasswordValidator<ApplicationUser>
{
    public Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user, string password)
    {
        if (user.PasswordUpdatedAt < DateTime.Now.AddDays(-180))
        {
            return Task.FromResult(IdentityResult.Failed(new IdentityError
            {
                Code = "ReplacePassword",
                Description = "Your password is older than 180 days, kindly update."
            }));
        }

        return Task.FromResult(IdentityResult.Success);
    }
}