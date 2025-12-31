using Microsoft.Extensions.Options;

namespace Syncie.Data.IO;

/// <summary>
/// Provides instances of <c>TFileStream</c> while performing lifetime control and disposal of the streams
/// in order to prevent leakage of unmanaged resources in potentially fatal cases of the streams.
/// </summary>
/// <remarks>This class shall be instantiated as a singleton.</remarks>
public sealed class ReadOnlyFileStreamProvider : IFileStreamProvider<ReadOnlyFileStream>
{
    private readonly IFileStreamProprietor<ReadOnlyFileStream> _streamProprietor;

    // ReSharper disable once ConvertToPrimaryConstructor
    public ReadOnlyFileStreamProvider(IFileStreamProprietor<ReadOnlyFileStream> streamProprietor)
    {
        _streamProprietor = streamProprietor;
    }

    /// <summary>
    /// Creates and gives read-only access to the file through a file stream.
    /// </summary>
    /// <param name="path">Absolute path to the file.</param>
    /// <param name="bufferSize">The size of the buffer used by FileStream for buffering. The default buffer size
    ///     is 8192. 0 or 1 means that buffering should be disabled. Negative values are not allowed.</param>
    /// <param name="options">A bitwise combination of the enumeration values that specifies additional file options.
    ///     The default value is Asynchronous + SequentialScan.</param>
    /// <returns>A managed instance of <c>ReadOnlyFileStream</c></returns>
    /// <remarks>Be aware that the stream expects to be read asynchronously by default. 
    /// Change <c>options</c> accordingly if synchronous operation is required,
    /// otherwise performance penalty is introduced.</remarks>
    public ReadOnlyFileStream New(string path, int bufferSize = 8192,
        FileOptions options = FileOptions.Asynchronous | FileOptions.SequentialScan)
    {
        var streamOptions = new FileStreamOptions()
        {
            Access = FileAccess.Read,
            Mode = FileMode.Open,
            Share = FileShare.Read,
            Options = options,
            BufferSize = bufferSize
        };

        return _streamProprietor.Borrow(path, streamOptions);
    }
    
    /// <summary>
    /// Creates and gives read-only access to the file through a file stream. The buffer size is auto-optimized
    /// by providing total amount of bytes that is going to be read.
    /// </summary>
    /// <param name="path">Absolute path to the file.</param>
    /// <param name="bytesToRead">Amount of bytes that is expected to be read in total.</param>
    /// <param name="options">A bitwise combination of the enumeration values that specifies additional file options.
    ///     The default value is Asynchronous + SequentialScan.</param>
    /// <returns>A managed instance of <c>ReadOnlyFileStream</c></returns>
    /// <remarks>Be aware that the stream expects to be read asynchronously by default. 
    /// Change <c>options</c> accordingly if synchronous operation is required,
    /// otherwise performance penalty is introduced.</remarks>
    public ReadOnlyFileStream NewOptimized(string path, long bytesToRead,
        FileOptions options = FileOptions.Asynchronous | FileOptions.SequentialScan)
    {
        var streamOptions = new FileStreamOptions()
        {
            Access = FileAccess.Read,
            Mode = FileMode.Open,
            Share = FileShare.Read,
            Options = options,
            BufferSize = OptimizeBufferSize(bytesToRead)
        };
        
        return _streamProprietor.Borrow(path, streamOptions);
    }
    
    /// <summary>
    /// Creates and gives read-only access to the file through a file stream. The buffer size is auto-optimized
    /// by the size of the whole file. This method shall be called in case the whole file is about to be read.
    /// </summary>
    /// <param name="path">Absolute path to the file.</param>
    /// <param name="options">A bitwise combination of the enumeration values that specifies additional file options.
    ///     The default value is Asynchronous + SequentialScan.</param>
    /// <returns>A managed instance of <c>ReadOnlyFileStream</c></returns>
    /// <remarks>Be aware that the stream expects to be read asynchronously by default. 
    /// Change <c>options</c> accordingly if synchronous operation is required,
    /// otherwise performance penalty is introduced.</remarks>
    public ReadOnlyFileStream NewOptimized(string path,
        FileOptions options = FileOptions.Asynchronous | FileOptions.SequentialScan)
    {
        var fileInfo = new FileInfo(path);
        var streamOptions = new FileStreamOptions()
        {
            Access = FileAccess.Read,
            Mode = FileMode.Open,
            Share = FileShare.Read,
            Options = options,
            BufferSize = OptimizeBufferSize(fileInfo.Length)
        };

        return _streamProprietor.Borrow(path, streamOptions);
    }

    private static int OptimizeBufferSize(long bytesToRead) => bytesToRead switch
    {
        <  4096        => 0,        // <  4096 B => 0 B
        <= 65536       => 4096,     // <= 64 KiB => 4 KiB
        <= 12_582_912  => 65536,    // <= 12 MiB => 64 KiB
        <= 50_331_648  => 262_144,  // <= 48 MiB => 256 KiB
        <= 100_663_296 => 524_288,  // <= 96 MiB => 512 KiB
                     _ => 1_048_576 // >  96 MiB => 1 MiB
    };

    
    // private sealed partial class ManagedReadOnlyFileStream;
}