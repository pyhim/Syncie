using System.Runtime.CompilerServices;
using Microsoft.Win32.SafeHandles;
using Syncie.Data.Partitioning;

namespace Syncie.Data.IO;

// ReSharper disable once InconsistentNaming
internal static class PartitionedFileIO
{
    internal static async IAsyncEnumerable<ReadOnlyMemory<byte>> ReadSectorAsync(SafeFileHandle handle,
        Memory<byte> bufferWindow, Sector sector, [EnumeratorCancellation] CancellationToken ct = default)
    {
        long cursor = sector.Start;
        long bytesRemaining = sector.Length;

        while (bytesRemaining > 0)
        {
            var bytesToRead = (int)Math.Min(bufferWindow.Length, bytesRemaining);
            var currentSlice = bufferWindow[..bytesToRead];

            var bytesRead = await RandomAccess.ReadAsync(handle, currentSlice, cursor, ct);
            yield return currentSlice[..bytesRead];
            
            cursor += bytesRead;
            bytesRemaining -= bytesRead;
        }
    }

    internal static async Task OverwriteSectorAsync(SafeFileHandle handle, ReadOnlyMemory<byte> bytesToWrite,
        Sector sector, int offset = 0, CancellationToken ct = default)
    {
        CheckDataOverflow(sector, bytesToWrite.Length, offset);
        long cursor = sector.Start + offset;
        
        await RandomAccess.WriteAsync(handle, bytesToWrite, cursor, ct);
    }

    private static void CheckDataOverflow(Sector sector, int bytesToWrite, int offset = 0)
    {
        if (bytesToWrite + offset > sector.Length) 
            throw new ArgumentException("The amount of to be written data overflows the sector.");
    }
}