using System.Security.Cryptography;
using Blake3;

namespace Syncie.Data.Cryptography;

public static class DataHasher
{
    internal static async Task<byte[]> HashWholeFileAsync(string filePath)
    {
        await using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        await using var hashStream = new Blake3Stream(fileStream, dispose: false);
        
        return hashStream.ComputeHash().AsSpan().ToArray();
    }

    internal static byte[] HashBlock(string filePath, int index)
    {
        throw new NotImplementedException();
    }
}