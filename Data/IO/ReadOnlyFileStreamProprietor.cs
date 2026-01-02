using System.Collections.Concurrent;

namespace Syncie.Data.IO;

public sealed partial class ReadOnlyFileStreamProprietor : IFileStreamProprietor<ReadOnlyFileStream>
{
    private readonly ConcurrentDictionary<Guid, ManagedReadOnlyFileStream> _openedStreams;

    /// <summary>
    /// Creates a pre-optimized instance of a class
    /// </summary>
    public ReadOnlyFileStreamProprietor()
    {
        const int capacity = 30;
        var concurrencyLevel = Environment.ProcessorCount * 2;
        _openedStreams = new ConcurrentDictionary<Guid, ManagedReadOnlyFileStream>(concurrencyLevel, capacity);
    }
    
    /// <summary>
    /// Creates an instance of a class with custom-defined parameters.
    /// </summary>
    /// <param name="capacity">The initial capacity of the dictionary</param>
    /// <param name="concurrencyLevel">The level of concurrency of the dictionary</param>
    /// <remarks>Designed to be used in QA.
    /// Consider using a parameterless, pre-optimized constructor if otherwise.</remarks>
    public ReadOnlyFileStreamProprietor(int capacity, int concurrencyLevel)
    {
        _openedStreams = new ConcurrentDictionary<Guid, ManagedReadOnlyFileStream>(concurrencyLevel, capacity);
    }
    
    public ReadOnlyFileStream Borrow(string path, FileStreamOptions streamOptions)
    {
        var newGuid = Guid.CreateVersion7();
        var newStream = new ManagedReadOnlyFileStream(newGuid, path, streamOptions);
        newStream.DisposeRequestHandler += HandleDisposeRequest;
        _openedStreams.TryAdd(newGuid, newStream);

        return newStream;
    }

    public void CleanDeadStreams(TimeSpan maxLifetime)
    {
        var deadStreams = _openedStreams.Where(pair =>
        {
            var timeOfInactivity = DateTime.Now - pair.Value.LastActivity;
            return timeOfInactivity > maxLifetime;
        }).ToList();
        
        deadStreams.ForEach(pair =>
        {
            _openedStreams[pair.Key].Dispose();
            _openedStreams.TryRemove(pair.Key, out _);
        });
    }

    private void HandleDisposeRequest(object? sender, DisposeRequestedArgs e)
    {
        _openedStreams.TryRemove(e.TrackingId, out _);
    }
    
    private sealed partial class ManagedReadOnlyFileStream;
}