using Microsoft.Win32.SafeHandles;
using Syncie.Data.Partitioning;

namespace Syncie.Data.IO;

public sealed class PartitionedFileWriter : IDisposable
{
    private readonly SafeFileHandle _handle;
    
    private PartitionedFile File { get; }
    
    public PartitionedFileWriter(PartitionedFile file)
    {
        File = file;
        _handle = FileHandleFactory.NewWriteOnly(file.Path);
    }

    public SectorWriter GetSectorWriter(Sector sector)
    {
        return new SectorWriter(_handle, sector);
    }

    public void Dispose()
    {
        _handle.Dispose();
    }
}