namespace Syncie.Data.IO;

/// <summary>
/// Provides instances of <see cref="FileStream"/>
/// </summary>
public static class FileStreamFactory
{
    /// <summary>
    /// Creates and gives read-only access to the file through a file stream.
    /// </summary>
    /// <param name="filePath">Absolute path to the file.</param>
    /// <param name="bufferSize">The size of the buffer used by FileStream for buffering. The default buffer size
    ///     is 8192. 0 or 1 means that buffering should be disabled. Negative values are not allowed.</param>
    /// <param name="options">A bitwise combination of enumeration values that specify additional file options.
    ///     The default value is Asynchronous + SequentialScan.</param>
    /// <returns>An instance of <see cref="FileStream"/></returns>
    /// <remarks>Be aware that the stream expects to be read asynchronously by default. 
    /// Change <c>options</c> accordingly if synchronous operation is required,
    /// otherwise performance penalty is introduced.</remarks>
    public static FileStream NewReadOnly(string filePath, int bufferSize = 8192,
        FileOptions options = FileOptions.Asynchronous | FileOptions.SequentialScan)
    {
        var fileStreamOptions = new FileStreamOptions()
        {
            Mode = FileMode.Open,
            Access = FileAccess.Read,
            Share = FileShare.Read,
            BufferSize = bufferSize,
            Options = options
        };
        var newStream = new FileStream(filePath, fileStreamOptions);
        
        return newStream;
    }
    
    /// <summary>
    /// Creates and gives read-only access to the file through a file stream. The buffer size is auto-optimized
    /// by providing total amount of bytes that is going to be read.
    /// </summary>
    /// <param name="filePath">Absolute path to the file.</param>
    /// <param name="bytesToRead">Amount of bytes that is expected to be read in total.</param>
    /// <param name="options">A bitwise combination of the enumeration values that specifies additional file options.
    ///     The default value is Asynchronous + SequentialScan.</param>
    /// <returns>An instance of <see cref="FileStream"/></returns>
    /// <remarks>Be aware that the stream expects to be read asynchronously by default. 
    /// Change <c>options</c> accordingly if synchronous operation is required,
    /// otherwise significant performance penalty is introduced.</remarks>
    public static FileStream NewReadOnly(string filePath, long bytesToRead,
        FileOptions options = FileOptions.Asynchronous | FileOptions.SequentialScan)
    {
        var fileStreamOptions = new FileStreamOptions()
        {
            Mode = FileMode.Open,
            Access = FileAccess.Read,
            Share = FileShare.Read,
            BufferSize = OptimizeBufferSize(bytesToRead),
            Options = options
        };
        var newStream = new FileStream(filePath, fileStreamOptions);

        return newStream;
    }
    
    /// <summary>
    /// Creates and gives read-only access to the file through a file stream. The buffer size is auto-optimized
    /// by the size of the whole file.
    /// </summary>
    /// <param name="filePath">Absolute path to the file.</param>
    /// <param name="options">A bitwise combination of the enumeration values that specifies additional file options.
    ///     The default value is Asynchronous + SequentialScan.</param>
    /// <returns>An instance of <see cref="FileStream"/></returns>
    /// <remarks>This method is advised to be called in case the <b>whole</b> file is about to be read.
    /// Be aware that the stream expects to be read asynchronously by default. 
    /// Change <c>options</c> accordingly if synchronous operation is required,
    /// otherwise significant performance penalty is introduced.</remarks>
    public static FileStream NewReadOnlyWhole(string filePath,
        FileOptions options = FileOptions.Asynchronous | FileOptions.SequentialScan)
    {
        var fileInfo = new FileInfo(filePath);
        var fileStreamOptions = new FileStreamOptions()
        {
            Mode = FileMode.Open,
            Access = FileAccess.Read,
            Share = FileShare.Read,
            BufferSize = OptimizeBufferSize(fileInfo.Length),
            Options = options
        };
        var newStream = new FileStream(filePath, fileStreamOptions);

        return newStream;
    }

    public static FileStream NewWritable(string filePath,
        FileOptions options = FileOptions.Asynchronous)
    {
        var fileStreamOptions = new FileStreamOptions()
        {
            Mode = FileMode.Open,
            Access = FileAccess.Write,
            Share = FileShare.Write,
            BufferSize = 0,
            Options = options
        };
        var newStream = new FileStream(filePath, fileStreamOptions);
        
        return newStream;
    }

    private static int OptimizeBufferSize(long bytesToRead) => bytesToRead switch
    {
        <  4096        => 0,        // <  4096 B => 0 B
        <= 65536       => 4096,     // <= 64 KiB => 4 KiB
        _              => 65536     // >  64 KiB => 64 KiB
        // <= 12_582_912  => 65536,    // <= 12 MiB => 64 KiB
        // <= 50_331_648  => 262_144,  // <= 48 MiB => 256 KiB
        // <= 100_663_296 => 524_288,  // <= 96 MiB => 512 KiB
        //              _ => 1_048_576 // >  96 MiB => 1 MiB
    };
}