using System.Runtime.CompilerServices;
using Microsoft.Win32.SafeHandles;
using Syncie.Data.Partitioning;
using Syncie.Utilities.Memory;

namespace Syncie.Data.IO;

public sealed class PartitionedFileReader : IDisposable
{
    private readonly SafeFileHandle _handle;
    
    private PartitionedFile File { get; }
    
    public PartitionedFileReader(PartitionedFile file)
    {
        File = file;
        _handle = FileHandleFactory.NewReadOnly(file.Path);
    }
    
    // ReSharper disable once AsyncMethodWithoutAwait
    public async IAsyncEnumerable<IAsyncEnumerable<ReadOnlyMemory<byte>>> ReadAllSectorsAsync(
        [EnumeratorCancellation] CancellationToken ct = default)
    {
        var bufferSize = BufferOptimizer.OptimizeBufferSize(File.Length);
        using var rawBuffer = new RentedArray<byte>(bufferSize);
        var bufferWindow = rawBuffer.AsMemory();

        foreach (var sector in File.Sectors)
        {
            yield return PartitionedFileIO.ReadSectorAsync(_handle, bufferWindow, sector, ct);
        }
    }
    
    /// <summary>
    /// Reads one specified sector of the file.
    /// </summary>
    /// <param name="sector"></param>
    /// <param name="ct"></param>
    /// <returns>Enumeration of bytes.</returns>
    /// <remarks>Ensure that the <see cref="Span{T}.CopyTo"/>
    /// is called from the returning memory in case you need to acquire ownership of the bytes,
    /// otherwise simple assignment leads to undefined behaviour.</remarks>>
    public async IAsyncEnumerable<ReadOnlyMemory<byte>> ReadSectorAsync(Sector sector,
        [EnumeratorCancellation] CancellationToken ct = default)
    {
        var bufferSize = BufferOptimizer.OptimizeBufferSize(File.Length);
        using var rawBuffer = new RentedArray<byte>(bufferSize);
        var bufferWindow = rawBuffer.AsMemory();

        await foreach (var bytes in PartitionedFileIO.ReadSectorAsync(_handle, bufferWindow, sector, ct))
        {
            yield return bytes;
        }
    }

    public void Dispose()
    {
        _handle.Dispose();
    }
}