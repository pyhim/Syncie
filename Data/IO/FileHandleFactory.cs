using Microsoft.Win32.SafeHandles;

namespace Syncie.Data.IO;

public static class FileHandleFactory
{
    public static SafeFileHandle NewReadOnly(string path,
        FileOptions options = FileOptions.Asynchronous | FileOptions.SequentialScan)
    {
        return File.OpenHandle(path, options: options);
    }

    public static SafeFileHandle NewWriteOnly(string path,
        FileOptions options = FileOptions.Asynchronous | FileOptions.SequentialScan)
    {
        return File.OpenHandle(path, FileMode.Open, FileAccess.Write, FileShare.None, options);
    }
}