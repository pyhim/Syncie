using Syncie.Data.Partitioning;

namespace Syncie.Data.IO;

public interface IFileReader
{
    /// <summary>
    /// Reads the given sector of the file. Optimized for random access.
    /// </summary>
    /// <param name="filePath">The full path to the file.</param>
    /// <param name="sector">The sector to be read.</param>
    /// <param name="maxBytesPerIteration">Maximum bytes per iteration. The default value is <c>8192</c></param>
    /// <returns></returns>
    IAsyncEnumerable<Memory<byte>> ReadSectorAsync(string filePath, Sector sector, int maxBytesPerIteration = 8192);
    
    /// <summary>
    /// Returns an async iterator for the file, yielding variable amount of bytes for each iteration originating
    /// from a file.
    /// </summary>
    /// <param name="filePath">The absolute path to the file.</param>
    /// <param name="maxBytesPerIteration">Maximum bytes per iteration. The default value is <c>8192</c></param>
    /// <returns></returns>
    IAsyncEnumerable<Memory<byte>> ReadAsync(string filePath, int maxBytesPerIteration = 8192);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="sectors"></param>
    /// <returns></returns>
    IAsyncEnumerable<IAsyncEnumerable<Memory<byte>>> ReadSectorsAsync(string filePath, Sectors sectors);
}