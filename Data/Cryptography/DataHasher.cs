using Syncie.Data.Hardware;
using Syncie.Data.IO;
using Syncie.Data.Partitioning;
using Syncie.Utilities.Memory;

namespace Syncie.Data.Cryptography;

public class DataHasher : IDataHasher
{
    public const int HashSize = 32;

    public async Task<Blake3.Hash> HashFileAsync(string filePath)
    {
        throw new NotImplementedException();
    }

    public async Task<TwoDimensionalRentedArray<byte>> HashFileSectorsAsync(string filePath, Sector[] sectors,
        SectorsAlignment alignment)
    {
        if (alignment == SectorsAlignment.Unknown)
            alignment = Sectors.EvaluateAlignment(sectors);

        return alignment switch
        {
            SectorsAlignment.Contiguous => await HashContiguousSectorsAsync(filePath, sectors),
            SectorsAlignment.Random => await HashNonContiguousSectorsAsync(filePath, sectors),
            SectorsAlignment.Unknown =>
                throw new ArgumentException("Unknown alignment of sectors in the array of sectors."),
            _ => throw new ArgumentException("Unknown alignment of sectors in the array of sectors.")
        };
    }

    /// <summary>
    /// Hash an array of bytes using single-thread BLAKE3 hash function.
    /// </summary>
    /// <param name="data">The data to compute hash from.</param>
    /// <returns>The 32-byte BLAKE3 checksum.</returns>
    /// <remarks>Use this to calculate a small amount of data (less than 128 KiB)</remarks>
    public static Blake3.Hash HashData(byte[] data)
    {
        return Blake3.Hasher.Hash(data.AsSpan());
    }

    private async Task<TwoDimensionalRentedArray<byte>> HashContiguousSectorsAsync(string filePath, Sector[] sectors)
    {
        var bytesToHash = sectors.Aggregate<Sector, long>(0, (current, sector) => current + sector.Length); // Summarizes lengths of all sectors
        var hashes = new TwoDimensionalRentedArray<byte>(sectors.Length, HashSize);
        var fileStoredOn = _fileInfoProvider.GetInfo(filePath).StoredOn;
    
        if (fileStoredOn == PhysicalDiskType.HardDiskDrive)
        {
            await Task.Run(() => HashContiguousSectors(filePath, sectors, hashes));
            return hashes;
        }
        
        await HashNonContiguousSectorsAsync(filePath, sectors);
        
        return hashes;
    }
    
    private async Task HashContiguousSectors(string filePath, Sector[] sectors, TwoDimensionalRentedArray<byte> hashOutput)
    {
        await using var fileStream = _fileStreamProvider.NewOptimized(filePath);
        
        const int bufferSize = 8192;
        var rawBuffer = new RentedArray<byte>(bufferSize);
        var buffer = rawBuffer.AsMemory();

        for (var i = 0; i < sectors.Length; i++)
        {
            
            var sector = sectors[i];
            var hashStorage = hashOutput.GetRowAsSpan(i);
            
            
            while (true)
            {
                var actualReadBytes = await fileStream.ReadAsync(buffer);
            }
        }
    }

    private async Task<TwoDimensionalRentedArray<byte>> HashNonContiguousSectorsAsync(string filePath, Sector[] sectors)
    {
        throw new NotImplementedException();
    }
}