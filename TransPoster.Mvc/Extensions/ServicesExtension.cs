using TransPoster.Mvc.Data.Products;
using TransPoster.Mvc.Services;

namespace TransPoster.Mvc.Extensions;

public static class ServicesExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IRoleService, RoleService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IDbManagementService, DbManagementService>();
        services.AddTransient<IMenuService, MenuService>();
        services.AddTransient(typeof(IDbModelsService<>), typeof(DbModelsService<>));
        services.AddTransient(typeof(ProductViewModelProccesser));

        return services;
    }
}