namespace Syncie.Data.Converters;

public static class BytesConverter
{
    public static decimal FromBytesToMegabytes(long bytes)
    {
        return bytes / 1024m / 1024m;
    }

    public static decimal FromBytesToKilobytes(long bytes)
    {
        return bytes / 1024m;
    }

    public static decimal FromKilobytesToBytes(decimal kilobytes)
    {
        return kilobytes * 1024m;
    }

    public static decimal FromMegabytesToBytes(decimal megabytes)
    {
        return megabytes * 1024m * 1024m;
    }
}