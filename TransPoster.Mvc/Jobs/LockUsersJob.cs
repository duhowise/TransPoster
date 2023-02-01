using Quartz;
using TransPoster.Mvc.Services;

namespace TransPoster.Mvc.Jobs;

public class LockUsersJob : IJob
{
    private readonly ILogger<LockUsersJob> _logger;
    private readonly IAuthService _authService;

    public LockUsersJob(ILogger<LockUsersJob> logger, IAuthService authService)
    {
        _logger = logger;
        _authService = authService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            _logger.LogInformation("LOCK USERS JOB STARTED");

            var users = await _authService.UsersWithLogin180DaysAgoAsync();

            foreach (var user in users)
            {
                await _authService.LockUserAsync(user.Id);
            }

            _logger.LogInformation("LOCK USERS JOB ENDED");
        }
        catch (Exception ex)
        {
            _logger.LogInformation("LOCK USERS JOB FAILED {Exception}", ex);

            throw;
        }
    }
}