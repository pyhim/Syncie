using System.Collections;

namespace Syncie.Data.Partitioning;

public class Sectors : IReadOnlyList<Sector>
{
    private Sector[] ArrayOfSectors
    {
        get;
        init
        {
            if (value.Length == 0)
                throw new ArgumentException($"Value cannot be an empty collection [{nameof(value)}]");
            field = value;
        }
    }
    
    public SectorsAlignment SectorsAlignment { get; init; }

    public Sector this[int index] => ArrayOfSectors[index];

    public int Count => ArrayOfSectors.Length;

    public Sectors(Sector[] sectors, SectorsAlignment sectorsAlignment)
    {
        ArrayOfSectors = sectors;
        SectorsAlignment = sectorsAlignment;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ArrayOfSectors.GetEnumerator();
    }

    public IEnumerator<Sector> GetEnumerator()
    {
        return ArrayOfSectors.AsEnumerable().GetEnumerator();
    }

    /// <summary>
    /// This method can evaluate the alignment of the sectors in the array.
    /// </summary>
    /// <param name="sectors">The array of sectors</param>
    /// <returns></returns>
    public static SectorsAlignment EvaluateAlignment(Sector[] sectors)
    {
        for (var i = 1; i < sectors.Length - 1; i++)
        {
            var previousSector = sectors[i - 1];
            var currentSector = sectors[i];
            var supposedCurrentStartIndex = previousSector.End + 1;

            if (supposedCurrentStartIndex != currentSector.Start)
                return SectorsAlignment.Random;
        }
        
        return SectorsAlignment.Contiguous;
    }
}