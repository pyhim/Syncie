using Microsoft.Extensions.Hosting;

namespace Syncie.Data.IO;

public class StreamProprietorCleanupService(IFileStreamProprietor<IFileStream> proprietor) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            proprietor.CleanDeadStreams(TimeSpan.FromMinutes(3));
            await Task.Delay(TimeSpan.FromMinutes(3), stoppingToken);
        }
    }
}