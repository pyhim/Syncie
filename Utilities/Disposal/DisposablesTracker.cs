using System.Collections.Concurrent;

namespace Syncie.Utilities.Disposal;

public sealed class DisposablesTracker : IDisposablesTracker
{
    private readonly ConcurrentDictionary<Guid, IDisposeManageable> _trackedStreams;

    /// <summary>
    /// Creates a pre-optimized instance of a class.
    /// </summary>
    public DisposablesTracker()
    {
        const int capacity = 30;
        var concurrencyLevel = Environment.ProcessorCount * 2;
        _trackedStreams = new ConcurrentDictionary<Guid, IDisposeManageable>(concurrencyLevel, capacity);
    }
    
    /// <summary>
    /// Creates an instance of a class with custom-defined parameters.
    /// </summary>
    /// <param name="capacity">The initial capacity of the dictionary.</param>
    /// <param name="concurrencyLevel">The level of concurrency of the dictionary.</param>
    /// <remarks>Designed to be used in QA.
    /// Consider using a parameterless, pre-optimized constructor if otherwise.</remarks>
    public DisposablesTracker(int capacity, int concurrencyLevel)
    {
        _trackedStreams = new ConcurrentDictionary<Guid, IDisposeManageable>(concurrencyLevel, capacity);
    }

    // public void Register(out Action<Guid> onDispose, out Guid streamId)
    // {
    //     onDispose = HandleDisposeRequest;
    //     streamId = Guid.CreateVersion7();
    // }

    public void Subscribe(IDisposeManageable stream)
    {
        var newGuid = Guid.CreateVersion7();
        stream.OnDispose = Unsubscribe;
        stream.TrackingId = newGuid;
        _trackedStreams.TryAdd(newGuid, stream);
    }

    public void CleanDeadStreams(TimeSpan maxLifetime)
    {
        var deadStreams = _trackedStreams.Where(pair =>
        {
            var timeOfInactivity = DateTime.Now - pair.Value.LastActivity;
            return timeOfInactivity > maxLifetime;
        }).ToList();
        
        deadStreams.ForEach(pair =>
        {
            _trackedStreams[pair.Key].Dispose();
            _trackedStreams.TryRemove(pair.Key, out _);
        });
    }

    private void Unsubscribe(Guid trackingId)
    {
        _trackedStreams.TryRemove(trackingId, out _);
    }
}