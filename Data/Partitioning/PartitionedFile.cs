using System.Runtime.CompilerServices;
using Syncie.Data.IO;
using Syncie.Utilities.Memory;

namespace Syncie.Data.Partitioning;

public sealed class PartitionedFile
{
    /// <summary>
    /// The absolute path to the file.
    /// </summary>
    public required string Path { get; init; }

    /// <summary>
    /// The size of the file in bytes.
    /// </summary>
    public required long Length { get; init; }

    public required Sectors Sectors { get; init; }

    // ReSharper disable once AsyncMethodWithoutAwait
    public async IAsyncEnumerable<IAsyncEnumerable<ReadOnlyMemory<byte>>> ReadAllSectorsAsync(
        [EnumeratorCancellation] CancellationToken ct = default)
    {
        var bufferSize = BufferOptimizer.OptimizeBufferSize(Length);
        using var rawBuffer = new RentedArray<byte>(bufferSize);
        var bufferWindow = rawBuffer.AsMemory();
        using var handle = FileHandleFactory.NewReadOnly(Path);

        foreach (var sector in Sectors)
        {
            yield return PartitionedFileIO.ReadSectorAsync(handle, bufferWindow, sector, ct);
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
        var bufferSize = BufferOptimizer.OptimizeBufferSize(Length);
        using var rawBuffer = new RentedArray<byte>(bufferSize);
        var bufferWindow = rawBuffer.AsMemory();
        using var handle = FileHandleFactory.NewReadOnly(Path);

        await foreach (var readSector in PartitionedFileIO.ReadSectorAsync(handle, bufferWindow, sector, ct))
        {
            yield return readSector;
        }
    }

    public async Task OverwriteSectorAsync(ReadOnlyMemory<byte> bytesToWrite, Sector sector, int offset = 0,
        CancellationToken ct = default)
    {
        using var handle = FileHandleFactory.NewWriteOnly(Path);

        await PartitionedFileIO.OverwriteSectorAsync(handle, bytesToWrite, sector, offset, ct);
    }
}