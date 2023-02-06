using Microsoft.AspNetCore.Identity;
using TransPoster.Data;
using TransPoster.Data.Identity;
using TransPoster.Mvc.Validators.Auth;

namespace TransPoster.Mvc.Extensions;

public static class IdentityExtension
{
    public static IServiceCollection AddIdentitySetup(this IServiceCollection services)
    {
        services.AddDefaultIdentity<ApplicationUser>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 7;

            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(2);
            options.Lockout.MaxFailedAccessAttempts = 5;

            options.SignIn.RequireConfirmedAccount = true;
        })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddPasswordValidator<ReplacePasswordValidator>();

        return services;
    }
}