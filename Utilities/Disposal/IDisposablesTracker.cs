namespace Syncie.Utilities.Disposal;

/// <summary>
/// Provides mechanism to create a stream tracker that disposes unmanaged resources of the tracked streams.
/// </summary>
public interface IDisposablesTracker
{
    /// <summary>
    /// Subscribes a new object to the tracker.
    /// </summary>
    /// <param name="stream">The stream to subscribe.</param>
    void Subscribe(IDisposeManageable stream);
    
    /// <summary>
    /// Ensures that the dead objects are properly disposed.
    /// </summary>
    /// <param name="maxLifetime">How long can an object be inactive.</param>
    void CleanDeadStreams(TimeSpan maxLifetime);
}