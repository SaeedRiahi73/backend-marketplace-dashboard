using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Task_Application.Contracts.Interfaces.RefreshTokens;

namespace Task_Infrastructure.BackgroundJobs;

public sealed class RefreshTokenCleanupBackgroundService : BackgroundService
{
    private static readonly TimeSpan CleanupInterval = TimeSpan.FromHours(24);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<RefreshTokenCleanupBackgroundService> _logger;

    public RefreshTokenCleanupBackgroundService(IServiceScopeFactory scopeFactory, ILogger<RefreshTokenCleanupBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await RunCleanupAsync(stoppingToken);

            try
            {
                await Task.Delay(CleanupInterval, stoppingToken);
            }
            catch (OperationCanceledException)
                when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
        }
    }

    private async Task RunCleanupAsync(CancellationToken cancellationToken)
    {
        try
        {
            await using AsyncServiceScope scope =
                _scopeFactory.CreateAsyncScope();

            IRefreshTokenCleanupService cleanupService =
                scope.ServiceProvider
                    .GetRequiredService<IRefreshTokenCleanupService>();

            int deletedCount = await cleanupService.DeleteExpiredAsync(
                DateTime.UtcNow,
                cancellationToken);

            _logger.LogInformation(
                "Refresh token cleanup completed. {DeletedCount} expired tokens were deleted.",
                deletedCount);
        }
        catch (OperationCanceledException)
            when (cancellationToken.IsCancellationRequested)
        {
            // Application shutdown requested.
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Refresh token cleanup failed.");
        }
    }
}
