namespace Syncie.Data.IO;

/// <summary>
/// Provides a generic view of a sequence of bytes that does not allow overwriting them. This is an interface.
/// </summary>
public interface IReadableStream : IStream
{
    /// <summary>
    /// Reads a block of bytes from the stream and writes the data in a given buffer.
    /// </summary>
    /// <param name="buffer">
    /// When this method returns, contains the specified byte array with the values between
    /// <c>offset</c> and <c>(offset + count - 1)</c> replaced by the bytes read from the current source.
    /// </param>
    /// <param name="offset">The byte offset in <c>array</c> at which the read bytes will be placed.</param>
    /// <param name="count">The maximum number of bytes to read.</param>
    /// <returns>
    /// The total number of bytes read into the buffer. This might be less than the number of bytes requested if
    /// that number of bytes are not currently available, or zero if the end of the stream is reached.
    /// </returns>
    int Read(byte[] buffer, int offset, int count);

    /// <summary>
    /// Reads a sequence of bytes from the current file stream and advances the position within the file stream
    /// by the number of bytes read.
    /// </summary>
    /// <param name="buffer">
    /// A region of memory. When this method returns, the contents of this region are replaced
    /// by the bytes read from the current file stream.
    /// </param>
    /// <returns>
    /// The total number of bytes read into the buffer. This can be less than the number of bytes allocated in the
    /// buffer if that many bytes are not currently available,
    /// or zero (0) if the end of the stream has been reached.
    /// </returns>
    int Read(Span<byte> buffer);

    /// <summary>
    /// Asynchronously reads a sequence of bytes from the current file stream and writes them to a byte array
    /// beginning at a specified offset, advances the position within the file stream by the number of bytes read,
    /// and monitors cancellation requests.
    /// </summary>
    /// <param name="buffer">The buffer to write the data into.</param>
    /// <param name="offset">The byte offset in buffer at which to begin writing data from the stream.</param>
    /// <param name="count">The maximum number of bytes to read.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous read operation and wraps the total number of bytes read into
    /// the buffer. The result value can be less than the number of bytes requested if the number of bytes currently
    /// available is less than the requested number,
    /// or it can be 0 (zero) if the end of the stream has been reached.
    /// </returns>
    Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously reads a sequence of bytes from the current file stream and writes them to a memory region,
    /// advances the position within the file stream by the number of bytes read, and monitors cancellation requests.
    /// </summary>
    /// <param name="buffer">The buffer to write the data into.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.
    /// The default value is None.
    /// </param>
    /// <returns>A task that represents the asynchronous read operation and wraps the total number of bytes read into
    /// the buffer. The result value can be less than the number of bytes requested if the number of bytes currently
    /// available is less than the requested number,
    /// or it can be 0 (zero) if the end of the stream has been reached.
    /// </returns>
    ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Sets the current position of this stream to the given value.
    /// </summary>
    /// <param name="offset">The point relative to <c>origin</c> from which to begin seeking.</param>
    /// <param name="origin">Specifies the beginning, the end, or the current position as a reference point
    /// for <c>offset</c>, using a value of type SeekOrigin.</param>
    /// <returns>The new position in the stream.</returns>
    long Seek(long offset, System.IO.SeekOrigin origin);
}