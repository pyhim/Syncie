using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Syncie.Data.Fragmentation;
using Syncie.Data.IO;

namespace Syncie.ClientApp;

internal static class Program
{
    internal static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddHostedService<StreamProprietorCleanupService>();
        builder.Services.AddSingleton<IFileStreamProprietor<ReadOnlyFileStream>, ReadOnlyFileStreamProprietor>();
        builder.Services.AddSingleton<IFileStreamProvider<ReadOnlyFileStream>, ReadOnlyFileStreamProvider>();
        
        builder.Services.AddSingleton<DataFragmentator>();
        
        using var host = builder.Build();
        var fragmentator = host.Services.GetService<DataFragmentator>();
    }
}