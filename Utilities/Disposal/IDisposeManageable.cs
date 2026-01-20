namespace Syncie.Utilities.Disposal;

/// <summary>
/// Provides a mechanism for tracking a stream and performing disposal of the unmanaged resources in fatal cases
/// of the stream.
/// </summary>
public interface IDisposeManageable : IDisposable
{
    /// <summary>
    /// A foreign function that is called by the object in order to notify the manager of this object about disposal.
    /// </summary>
    Action<Guid>? OnDispose { set; }
    
    /// <summary>
    /// The last time since any action has been performed with the object.
    /// </summary>
    DateTime LastActivity { get; }
    
    /// <summary>
    /// The tracking ID given by the manager of this object.
    /// </summary>
    Guid? TrackingId { set; }
}