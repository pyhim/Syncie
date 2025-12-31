namespace Syncie.Data.Fragmentation;

public record Block(int Index, int Start, int End) : Sector(Start, End)
{
    public byte[] Checksum { get; init; } = [];
    public int Length => End - Start;
}