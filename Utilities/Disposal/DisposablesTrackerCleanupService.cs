using Microsoft.Extensions.Hosting;

namespace Syncie.Utilities.Disposal;

public class DisposablesTrackerCleanupService(IDisposablesTracker tracker) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var delay = TimeSpan.FromMinutes(5);
            tracker.CleanDeadStreams(delay);
            await Task.Delay(delay, stoppingToken);
        }
    }
}