namespace Syncie.Data.IO;

public interface IFileStreamProvider<out TFileStream> where TFileStream : IFileStream
{
    TFileStream New(string path, int bufferSize = 8192,
        FileOptions options = FileOptions.Asynchronous | FileOptions.SequentialScan);

    TFileStream NewOptimized(string path, long bytesToRead,
        FileOptions options = FileOptions.Asynchronous | FileOptions.SequentialScan);

    TFileStream NewOptimized(string path,
        FileOptions options = FileOptions.Asynchronous | FileOptions.SequentialScan);
}