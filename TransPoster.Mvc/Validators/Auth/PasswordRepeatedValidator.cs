using Microsoft.AspNetCore.Identity;
using TransPoster.Data.Models;

namespace TransPoster.Mvc.Validators.Auth;

public class PasswordRepeatedValidator : IPasswordValidator<ApplicationUser>
{
    public Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user,
        string password)
    {
        var isPartOfLast5Passwords = user
            .UserHistory
            .Select(h => h.HashedPassword)
            .Distinct()
            .Any(hashedPassword =>
                manager
                    .PasswordHasher
                    .VerifyHashedPassword(user, hashedPassword, password) is PasswordVerificationResult.Success
            );

        if (isPartOfLast5Passwords)
        {
            return Task.FromResult(IdentityResult.Failed(new IdentityError
            {
                Code = "PasswordRepeated",
                Description = "Password has been used before"
            }));
        }

        return Task.FromResult(IdentityResult.Success);
    }
}