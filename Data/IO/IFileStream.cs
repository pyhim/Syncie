namespace Syncie.Data.IO;

public interface IFileStream : IStream
{
    /// <summary>
    /// Gets a value that indicates whether the stream was opened asynchronously or synchronously.
    /// </summary>
    bool IsAsync { get; }

    /// <summary>
    /// A string that is the absolute path of the file.
    /// </summary>
    string Name { get; }
}