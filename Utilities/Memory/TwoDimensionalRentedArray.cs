using System.Buffers;
using System.Collections;

namespace Syncie.Utilities.Memory;

/// <summary>
/// Represents a two-dimensional array that is rented from an array pool.
/// </summary>
/// <typeparam name="T">The type of array</typeparam>
/// <remarks>This class is actually implemented as a 1D array but with 2D convenience tools.</remarks>
public sealed class TwoDimensionalRentedArray<T> : ICollection, ITwoDimensionalCollection<T>, IEnumerable<Span<T>>,
    IDisposable
{
    private readonly T[] _array;
    
    public int Rows { get; }
    public int Columns { get; }
    public int Count => Rows * Columns;
    public bool IsSynchronized => _array.IsSynchronized; // Note: Always false.
    public object SyncRoot => _array.SyncRoot;

    public T this[int row, int column]
    {
        get
        {
            ThrowIfIndexOutOfRange(row * column);
            return _array[row * column];
        }
        set
        {
            ThrowIfIndexOutOfRange(row * column);
            _array[row * column] = value;
        }
    }

    /// <summary>
    /// Initializes a new two-dimensional array that is rented from an <see cref="ArrayPool{T}"/>.
    /// </summary>
    /// <param name="rows">The amount of rows.</param>
    /// <param name="columns">The amount of columns.</param>
    public TwoDimensionalRentedArray(int rows, int columns)
    {
        _array = ArrayPool<T>.Shared.Rent(rows * columns);
        Rows = rows;
        Columns = columns;
    }

    public Span<T> GetRowAsSpan(int row) => _array.AsSpan(row, Columns);

    private void ThrowIfIndexOutOfRange(int index)
    {
        if (index < 0 || index > Count)
            throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
    }

    public void CopyTo(Array array, int index) => _array.CopyTo(array, index);
    
    public void Dispose()
    {
        lock (_array)
        {
            ArrayPool<T>.Shared.Return(_array, clearArray: true);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Gets the enumerator for the rows of the array.
    /// </summary>
    /// <returns>An enumerator instance.</returns>
    public IEnumerator<Span<T>> GetEnumerator()
    {
        return new Enumerator(_array, Columns);
    }

    private class Enumerator(T[] array, int columns) : IEnumerator<Span<T>>
    {
        private int _index = -columns;

        public bool MoveNext()
        {
            _index += columns;

            return _index < array.Length;
        }

        public void Reset() => _index = -columns;

        Span<T> IEnumerator<Span<T>>.Current => Current;

        object? IEnumerator.Current => new Memory<T>(array, _index, columns);

        private Span<T> Current => new(array, _index, columns);

        public void Dispose()
        {
        }
    }
}