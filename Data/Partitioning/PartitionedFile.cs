using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using Syncie.Data.IO;
using Syncie.Utilities.Memory;

namespace Syncie.Data.Partitioning;

public sealed record PartitionedFile
{
    public ImmutableArray<Sector> Sectors { get; }
    
    /// <summary>
    /// The absolute path to the file.
    /// </summary>
    public string Path { get; }

    /// <summary>
    /// The size of the file in bytes.
    /// </summary>
    public long Length { get; }

    /// <summary>
    /// Creates a partitioned file metadata.
    /// </summary>
    /// <param name="path">Absolute path to the file.</param>
    /// <param name="length">The length of the file.</param>
    /// <param name="sectors"><b>Sorted</b> array of <b>unique</b> sectors.</param>
    internal PartitionedFile(string path, long length, Sector[] sectors)
    {
        Path = path;
        Length = length;
        // ReSharper disable once UseCollectionExpression
        Sectors = sectors.ToImmutableArray();
    }
    
    public PartitionedFile(string path, long length, SortedSet<Sector> sectors)
    {
        Path = path;
        Length = length;
        // ReSharper disable once UseCollectionExpression
        Sectors = sectors.ToImmutableArray();
    }
}