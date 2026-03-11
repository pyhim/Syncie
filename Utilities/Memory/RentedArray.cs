using System.Buffers;
using System.Collections;

namespace Syncie.Utilities.Memory;

public sealed class RentedArray<T> : IMemoryOwner<T>
{
    private readonly T[] _array;

    public int Length { get; }
    public Memory<T> Memory => AsMemory();

    /// <summary>
    /// Creates an array that is rented from an <see cref="ArrayPool{T}"/>
    /// </summary>
    /// <param name="length">The desirable length of the array.</param>
    public RentedArray(int length)
    {
        _array = ArrayPool<T>.Shared.Rent(length);
        Length = length;
    }

    public Memory<T> AsMemory() => new(_array, 0, Length);

    public Memory<T> AsMemory(int length) => new(_array, 0, length);
    
    public Span<T> AsSpan() => new(_array, 0, Length);
    
    public Span<T> AsSpan(int length) => new(_array, 0, length);

    public void Dispose()
    {
        ArrayPool<T>.Shared.Return(_array, clearArray: true);
    }
}