using Quartz;
using TransPoster.Mvc.Jobs;

namespace TransPoster.Mvc.Extensions;

public static class QuartzExtension
{
    public static IServiceCollection AddQuartzSetup(this IServiceCollection services)
    {
        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionScopedJobFactory();

            var jobKey = new JobKey("LockUsersJob");
            q.AddJob<LockUsersJob>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("LockUsersJob-trigger")
                //Daily everyday at 12 am
                .WithCronSchedule("0 0 0 1/1 * ? *")
            );
        });

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

        return services;
    }
}