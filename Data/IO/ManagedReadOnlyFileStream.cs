namespace Syncie.Data.IO;

public sealed partial class ReadOnlyFileStreamProprietor
{
    /// <summary>
    /// Represents a file stream whose unmanaged resources are partially controlled by another class
    /// </summary>
    private sealed partial class ManagedReadOnlyFileStream(Guid trackingId, string path, FileStreamOptions options)
        : ReadOnlyFileStream(path, options)
    {   
        public event EventHandler<DisposeRequestedArgs>? DisposeRequested;
        public Guid TrackingId { get; } = trackingId;
        public DateTime LastActivity { get; private set; } = DateTime.Now;

        public override int Read(byte[] buffer, int offset, int count)
        {
            LastActivity = DateTime.Now;
            return base.Read(buffer, offset, count);
        }

        public override int Read(Span<byte> buffer)
        {
            LastActivity = DateTime.Now;
            return base.Read(buffer);
        }

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count,
            CancellationToken cancellationToken = default)
        {
            LastActivity = DateTime.Now;
            return base.ReadAsync(buffer, offset, count, cancellationToken);
        }

        public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            LastActivity = DateTime.Now;
            return base.ReadAsync(buffer, cancellationToken);
        }

        private void OnDisposeRequested(DisposeRequestedArgs args)
        {
            DisposeRequested?.Invoke(this, args);
        }

        public override void Dispose()
        {
            if (Disposed) return;
            Stream.Dispose();
            Disposed = true;
            OnDisposeRequested(new DisposeRequestedArgs());
        }

        public override ValueTask DisposeAsync()
        {
            if (Disposed) return default;
            Disposed = true;
            OnDisposeRequested(new DisposeRequestedArgs());
            return Stream.DisposeAsync();
        }
    }
}