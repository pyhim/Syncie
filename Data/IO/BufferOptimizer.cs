namespace Syncie.Data.IO;

public static class BufferOptimizer
{
    /// <summary>
    /// Optimizes the size of buffer in case the whole file is to be read.
    /// </summary>
    /// <param name="filePath">The absolute path to the file.</param>
    /// <returns>Optimized size of the buffer.</returns>
    public static int OptimizeBufferSize(string filePath)
    {
        var bytesToRead = new FileInfo(filePath).Length;

        return bytesToRead switch
        {
            <= 4096 => 4096, // <  4096 B => 0 B
            <= 65536 => 8192, // <= 64 KiB => 4 KiB
            <= 12_582_912 => 65536, // <= 12 MiB => 64 KiB
            <= 50_331_648 => 262_144, // <= 48 MiB => 256 KiB
            <= 100_663_296 => 524_288, // <= 96 MiB => 512 KiB
            _ => 1_048_576 // >  96 MiB => 1 MiB
        };
    }

    /// <summary>
    /// Optimizes the size of buffer.
    /// </summary>
    /// <param name="bytesToRead">The amount of bytes to be read.</param>
    /// <returns>Optimized size of the buffer.</returns>
    public static int OptimizeBufferSize(long bytesToRead) => bytesToRead switch
    {
        <= 4096 => 4096, // <  4096 B => 0 B
        <= 65536 => 8192, // <= 64 KiB => 4 KiB
        <= 12_582_912 => 65536, // <= 12 MiB => 64 KiB
        <= 50_331_648 => 262_144, // <= 48 MiB => 256 KiB
        <= 100_663_296 => 524_288, // <= 96 MiB => 512 KiB
        _ => 1_048_576 // >  96 MiB => 1 MiB
    };
}