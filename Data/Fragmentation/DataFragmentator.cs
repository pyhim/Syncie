using System.Buffers;
using System.Collections.Frozen;
using Microsoft.Extensions.Options;
using Syncie.Data.Cryptography;
using Syncie.Data.IO;

namespace Syncie.Data.Fragmentation;

/// <summary>
/// A utility for breaking down any kind of data into blocks.
/// </summary>
public sealed class DataFragmentator
{
    private const int MaxBlockAmountPerFile = 64;
    private readonly IFileStreamProvider<IReadableStream> _fileStreamProvider;

    // ReSharper disable once ConvertToPrimaryConstructor
    public DataFragmentator(IFileStreamProvider<IReadableStream> provider)
    {
        _fileStreamProvider = provider;
    }

    public async Task<FragmentatedFile> FragmentateAsync(FileInfo fileInfo)
    {
        await using var fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
        var fileSizeTier = FileSizeClassifier.Classify(fileInfo.Length);

        if (fileSizeTier == FileSizeTier.Insignificant)
        {
            return await ProcessFileWithoutFragmentationAsync(fileInfo);
        }

        var blockSize = BlockSize[fileSizeTier];
        var totalBlocksCount = Math.Ceiling((double)fileInfo.Length / blockSize);
        var processedBlocks = new Block[fileInfo.Length / blockSize];
        const int bufferSize = 8192;
        var rawBuffer = ArrayPool<byte>.Shared.Rent(bufferSize);
        var selectedBytes = new Memory<byte>(rawBuffer, 0, bufferSize);
        int readBytesCount;

        while ((readBytesCount = await fileStream.ReadAsync(selectedBytes)) > 0)
        {
            ReadOnlySpan<byte> slice = selectedBytes.Span.Slice(0, readBytesCount);
            // TODO: Implement multi-threaded fragmentation
        }

        ArrayPool<byte>.Shared.Return(rawBuffer);
    }

    // private async Task<FragmentatedFile> FragmentateFileAsync(FileInfo fileInfo)
    // {
    //     
    // } 

    // Since files lower than 8192 KB are not eligible for fragmentation
    // class must process the file anyway, thus only performing hashing and
    // building the return object
    private static async Task<FragmentatedFile> ProcessFileWithoutFragmentationAsync(FileInfo fileInfo)
    {
        var hash = DataHasher.HashWholeFileAsync(fileInfo.FullName);
        var block = new Block(0, 0, (int)fileInfo.Length) { Checksum = await hash };

        var fragmentedFile = new FragmentatedFile
        {
            Id = Guid.Empty, // TODO: Decide whether ID is needed
            Path = fileInfo.FullName,
            Size = fileInfo.Length,
            BlocksCount = 1,
            BlockSize = (int)fileInfo.Length,
            Blocks = [block]
        };

        return fragmentedFile;
    }

    /// <summary>
    /// A dictionary serving purpose of a block size enumeration bound
    /// to the file size
    /// </summary>
    private static readonly FrozenDictionary<FileSizeTier, int> BlockSize = new Dictionary<FileSizeTier, int>()
    {
        { FileSizeTier.Insignificant, 0 },
        { FileSizeTier.Micro, 8192 },
        { FileSizeTier.Tiny, 65536 },
        { FileSizeTier.Small, 262144 },
        { FileSizeTier.Medium, 786432 },
        { FileSizeTier.Big, 1572864 },
        { FileSizeTier.Large, 4194304 },
        { FileSizeTier.Gigantic, 16777216 },
    }.ToFrozenDictionary();
}