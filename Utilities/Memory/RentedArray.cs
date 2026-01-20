using System.Buffers;
using System.Collections;

namespace Syncie.Utilities.Memory;

public sealed class RentedArray<T> : ICollection, IRentedArray<T>, IList<T>, IDisposable
{
    private readonly T[] _array;
    
    public int Count { get; }
    public bool IsSynchronized => _array.IsSynchronized; // Note: Always false.
    public object SyncRoot => _array.SyncRoot;
    public bool IsReadOnly => false;

    public T this[int index]
    {
        get
        {
            ThrowIfIndexOutOfRange(index);
            return _array[index];
        }
        
        set
        {
            ThrowIfIndexOutOfRange(index);
            _array[index] = value;
        }
    }

    public RentedArray(int length)
    {
        _array = ArrayPool<T>.Shared.Rent(length);
        Count = length;
    }
    
    public Memory<T> AsMemory() => new(_array, 0, Count);

    public void Add(T item) => throw new NotSupportedException();

    public void Clear() => ((IList<T>)_array).Clear();

    public bool Contains(T item) => _array.Contains(item);
    
    public bool Remove(T item) => ((IList<T>)_array).Remove(item);

    public int IndexOf(T item) => _array.IndexOf(item);

    public void Insert(int index, T item) => ((IList<T>)_array).Insert(index, item);

    public void RemoveAt(int index) => ((IList<T>)_array).RemoveAt(index);
    
    public void CopyTo(T[] array, int arrayIndex) => _array.CopyTo(array, arrayIndex);

    public void CopyTo(Array array, int index) => _array.CopyTo(array, index);

    public void Dispose()
    {
        lock (_array)
        {
            ArrayPool<T>.Shared.Return(_array, clearArray: true);
        }
    }

    private void ThrowIfIndexOutOfRange(int index)
    {
        if (index < 0 || index > Count)
            throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerator<T> GetEnumerator()
    {
        return new Enumerator(this);
    }

    private class Enumerator(RentedArray<T> array) : IEnumerator<T>
    {
        private int _index = -1;

        public bool MoveNext() => ++_index < array.Count;

        public void Reset()
        {
            _index = -1;
        }

        object? IEnumerator.Current => array[_index];

        T IEnumerator<T>.Current => array[_index];

        public void Dispose()
        {
        }
    }
}