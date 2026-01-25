namespace Syncie.Data.Partitioning;

/// <summary>
/// A utility for breaking down files into sectors.
/// </summary>
public static class FilePartitioner
{
    public static PartitionedFile Partition(string filePath)
    {
        var fileInfo = new FileInfo(filePath);
        var fileSizeTier = FileSizeClassifier.Classify(fileInfo.Length);
        int defaultSectorLength;

        if (fileSizeTier == FileSizeTier.Insignificant) 
            defaultSectorLength = (int)fileInfo.Length;
        else 
            defaultSectorLength = EvaluateOptimalSectorLength(fileSizeTier);
        
        var sectorsCount = (int)Math.Ceiling((double) fileInfo.Length / defaultSectorLength);
        var sectors = new Sector[sectorsCount];

        var remainingSpace = fileInfo.Length;
        var start = 0;
        for (var i = 0;; i++)
        {
            if (remainingSpace <= defaultSectorLength)
            {
                sectors[i] = new Sector(start, (int)remainingSpace);
                break;
            }

            sectors[i] = new Sector(start, defaultSectorLength);
            start += defaultSectorLength;
            remainingSpace -= defaultSectorLength;
        }

        return new PartitionedFile
        {
            Path = fileInfo.FullName,
            Length = fileInfo.Length,
            Sectors = new Sectors(sectors, SectorsAlignment.Contiguous) // This method guarantees to return only contiguous sectors.
        };
    }

    private static int EvaluateOptimalSectorLength(FileSizeTier fileSizeTier) => fileSizeTier switch
    {
        FileSizeTier.Micro => 8192, // 8 KiB
        FileSizeTier.Tiny => 65536, // 64 KiB
        FileSizeTier.Small => 262144, // 256 KiB
        FileSizeTier.Medium => 786432, // 768 KiB
        FileSizeTier.Big => 1572864, // 1.5 MiB
        FileSizeTier.Large => 4194304, // 4 MiB
        FileSizeTier.Gigantic => 16777216, // 16 MiB
        _ => 0
    };
}