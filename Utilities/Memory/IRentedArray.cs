namespace Syncie.Utilities.Memory;

public interface IRentedArray<T>
{
    Memory<T> AsMemory();
}