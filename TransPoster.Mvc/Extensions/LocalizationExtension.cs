using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace TransPoster.Mvc.Extensions;

public static class LocalizationExtension
{
    public static IServiceCollection AddLocalizationSetup(this IServiceCollection services)
    {
        services.AddLocalization(opt =>
        {
            opt.ResourcesPath = "Resources";
        });

        services.Configure<RequestLocalizationOptions>(options =>
        {
            List<CultureInfo> supportedCultures = new()
            {
                new CultureInfo("en-US"),
                new CultureInfo("he-IL"),
            };

            options.DefaultRequestCulture = new RequestCulture("en-US");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });

        return services;
    }
}