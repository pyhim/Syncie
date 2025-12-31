namespace Syncie.Data.IO;

/// <summary>
/// Provides a generic view of a sequence of bytes that does not allow overwriting them. This is an abstract class.
/// </summary>
public class ReadOnlyFileStream : IReadableStream, IFileStream
{
    protected FileStream Stream { get; }
    protected bool Disposed { get; set; }

    public bool CanSeek => Stream.CanSeek;
    public bool CanWrite => Stream.CanWrite;
    public bool CanRead => Stream.CanRead;
    public bool IsAsync => Stream.IsAsync;
    public long Length => Stream.Length;
    public string Name => Stream.Name;

    public virtual long Position
    {
        get => Stream.Position;
        set => Stream.Position = value;
    }
    
    public ReadOnlyFileStream(string path, int bufferSize, FileOptions options)
    {
        Stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, options);
    }

    protected ReadOnlyFileStream(string path, FileStreamOptions options)
    {
        Stream = new FileStream(path, options);
    }

    public virtual int Read(byte[] buffer, int offset, int count)
    {
        return Stream.Read(buffer, offset, count);
    }

    public virtual int Read(Span<byte> buffer)
    {
        return Stream.Read(buffer);
    }

    public virtual Task<int> ReadAsync(byte[] buffer, int offset, int count,
        CancellationToken cancellationToken = default)
    {
        return Stream.ReadAsync(buffer, offset, count, cancellationToken);
    }

    public virtual ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        return Stream.ReadAsync(buffer, cancellationToken);
    }

    public long Seek(long offset, SeekOrigin origin)
    {
        return Stream.Seek(offset, origin);
    }

    public virtual void Dispose()
    {
        GC.SuppressFinalize(this);
        Stream.Dispose();
    }

    public virtual ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return Stream.DisposeAsync();
    }
}