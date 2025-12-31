namespace Syncie.Data.IO;

public interface IFileStreamProprietor<out TFileStream> where TFileStream : IFileStream
{
    TFileStream Borrow(string path, FileStreamOptions options);
    
    /// <summary>
    /// Ensures that even dead streams are disposed and deleted from proprietor
    /// </summary>
    /// <param name="maxLifetime">How long can a stream be inactive</param>
    void CleanDeadStreams(TimeSpan maxLifetime);
}