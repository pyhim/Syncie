using System.Buffers.Text;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Syncie.Data.IO;
using Syncie.Data.Partitioning;

namespace Syncie.ClientApp;

internal static class Program
{
    internal static async Task Main(string[] args)
    {
        // var builder = Host.CreateApplicationBuilder(args);
        //
        // builder.Services.AddHostedService<DisposablesTrackerCleanupService>();
        // builder.Services.AddSingleton<IDisposablesTracker, DisposablesTracker>();
        // builder.Services.AddSingleton<IFileStreamProvider<ReadOnlyFileStream>, ReadOnlyFileStreamProvider>();
        //
        // using var host = builder.Build();
        // host.Run();

        // var sectors = FilePartitioner.Partition("/home/dmytro/Games/Monifactory Server/mods/cloth-config-11.1.136-forge.jar");
        // var sectors = FilePartitioner.Partition("lorem.txt");
        
        // var disposablesTracker = new DisposablesTracker();
        // var fileStreamProvider = new ReadOnlyFileStreamProvider(disposablesTracker);
        // var fileReader = new FileReader(fileStreamProvider);
        // var builder = new StringBuilder();
        //
        // int i = 0, k = 0;
        // await foreach (var bytes in fileReader.ReadAsync("lorem.txt"))
        // {
        //     i++;
        //     foreach (var b in bytes.Span)
        //     {
        //         builder.Append(Convert.ToChar(b));
        //         k++;
        //     }
        // }
        //
        // Console.WriteLine(builder.ToString());
        // Console.WriteLine(i);
        // Console.WriteLine(k);
        
        var partitionedFile = FilePartitioner.Partition(
            "/home/dmytro/Projects/Rider/Syncie/ClientApp/lorem.txt");

        var sector = partitionedFile.Sectors[0];
        var garbageData = new byte[64];

        for (var i = 0; i < 64; i++)
        {
            garbageData[i] = Convert.ToByte('q');
        }

        var window = garbageData.AsMemory();

        await partitionedFile.OverwriteSectorAsync(window, sector);

        // await foreach (var sector in partitionedFile.ReadAllSectorsAsync())
        // {
        //     await foreach (var bytes in sector)
        //     {
        //         foreach (var b in bytes.Span)
        //         {
        //             Console.Write(Convert.ToChar(b));
        //         }
        //     }
        // }
    }
}