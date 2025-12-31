namespace Syncie.Data.Fragmentation;

public record FragmentatedFile
{
    public required string Path { get; init; }
    // TODO: Decide whether ID is needed
    public required Guid Id { get; init; } 
    public required int BlocksCount { get; init; }
    public required int BlockSize { get; init; }
    public required long Size { get; init; }
    public Block[] Blocks { get; init; } = [];
}