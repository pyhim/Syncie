using Microsoft.Win32.SafeHandles;
using Syncie.Data.Partitioning;

namespace Syncie.Data.IO;

public class SectorWriter
{
    private readonly SafeFileHandle _handle;
    private int _offset;

    public Sector Sector { get; }
    
    internal SectorWriter(SafeFileHandle fileHandle, Sector sector)
    {
        Sector = sector;
        _handle = fileHandle;
    }

    public async Task PushData(ReadOnlyMemory<byte> bytesToWrite)
    {
        var cursor = _offset + Sector.Start;
        
        if (cursor >= Sector.End) 
            throw new InvalidOperationException("There is no more space left in the sector to be written.");

        await PartitionedFileIO.OverwriteSectorAsync(_handle, bytesToWrite, Sector, cursor);
        
        _offset += bytesToWrite.Length;
    }
}