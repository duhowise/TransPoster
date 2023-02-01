using TransPoster.Mvc.Services;

namespace TransPoster.Mvc.Extensions;

public static class ServicesExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IRoleService, RoleService>();

        return services;
    }
}