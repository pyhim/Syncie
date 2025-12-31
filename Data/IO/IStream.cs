namespace Syncie.Data.IO;

public interface IStream : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Gets a value that indicates whether the current stream supports seeking.
    /// </summary>
    bool CanSeek { get; }
    
    /// <summary>
    /// Gets a value that indicates whether the current stream supports writing.
    /// </summary>
    bool CanWrite { get; }
    
    /// <summary>
    /// Gets a value that indicates whether the current stream supports reading.
    /// </summary>
    bool CanRead { get; }
    
    /// <summary>
    /// Gets the length in bytes of the stream.
    /// </summary>
    long Length { get; }
    
    /// <summary>
    /// Gets or sets the current position of this stream.
    /// </summary>
    long Position { get; set; }
}