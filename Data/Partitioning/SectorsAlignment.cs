namespace Syncie.Data.Partitioning;

/// <summary>
/// An enumeration that represents the way sectors are aligned in the array.
/// </summary>
public enum SectorsAlignment
{
    /// <summary>
    /// The alignment has not been evaluated.
    /// </summary>
    Unknown,
    
    /// <summary>
    /// The alignment of the sectors is contiguous.
    /// </summary>
    Contiguous,
    
    /// <summary>
    /// The alignment of the sectors is random.
    /// </summary>
    Random
}