namespace Syncie.Utilities.Memory;

public interface ITwoDimensionalCollection<T>
{
    Span<T> GetRowAsSpan(int row);
}