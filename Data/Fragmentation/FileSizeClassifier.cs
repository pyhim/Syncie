namespace Syncie.Data.Fragmentation;

internal static class FileSizeClassifier
{
    // ReSharper disable once InconsistentNaming
    private const long KB = 1024;

    // ReSharper disable once InconsistentNaming
    private const long MB = KB * 1024;

    private const long InsignificantThreshold = 8 * KB;
    private const long MicroThreshold         = 512 * KB;
    private const long TinyThreshold          = 4 * MB;
    private const long SmallThreshold         = 16 * MB;
    private const long MediumThreshold        = 48 * MB;
    private const long BigThreshold           = 96 * MB;
    private const long LargeThreshold         = 256 * MB;

    internal static FileSizeTier Classify(long fileSize) => fileSize switch
    {
        <= InsignificantThreshold => FileSizeTier.Insignificant,
        <= MicroThreshold         => FileSizeTier.Micro,
        <= TinyThreshold          => FileSizeTier.Tiny,
        <= SmallThreshold         => FileSizeTier.Small,
        <= MediumThreshold        => FileSizeTier.Medium,
        <= BigThreshold           => FileSizeTier.Big,
        <= LargeThreshold         => FileSizeTier.Large,
                                _ => FileSizeTier.Gigantic
    };
}