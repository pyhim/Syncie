namespace Syncie.Data.Fragmentation;

internal enum FileSizeTier
{
    /// <remarks>File size is less than 8KB and insufficient to be fragmentated.</remarks>
    Insignificant,
    /// <remarks>From 8KB to 512KB.</remarks>
    Micro,
    /// <remarks>From 512KB to 4M.</remarks>
    Tiny,
    /// <remarks>From 4MB to 16MB.</remarks>
    Small,
    /// <remarks>From 16MB to 48MB.</remarks>
    Medium,
    /// <remarks>From 48MB to 96MB.</remarks>
    Big,
    /// <remarks>From 96MB to 256MB.</remarks>
    Large,
    /// <remarks>Bigger than 256MB.</remarks>
    Gigantic
}