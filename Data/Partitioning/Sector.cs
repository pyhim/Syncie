namespace Syncie.Data.Partitioning;

public readonly record struct Sector(int Start = 0, int Length = 0) : IComparable<Sector>
{
    /// <summary>
    /// Returns the including end (that means that the end of the sector is in its reach).
    /// </summary>
    public int End => Start + Length - 1;

    public int CompareTo(Sector other)
    {
        return Start.CompareTo(other.Start);
    }
}
